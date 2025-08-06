using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.Browser.CustomTabs;
using Autofac;
using Castle.Core.Internal;
using Com.Airbnb.Lottie;
using Com.EightbitLab.BlurViewBinding;
using ReactiveUI;
using System;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.Common.Config;
using TSD.PreviewDemo.Core.Users;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.ViewModel.Login;
using Xamarin.Essentials;
#pragma warning disable CS0618 // Type or member is obsolete

// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace TSD.PreviewDemo.App.Activities.Login
{
    [BusinessProcessState("Сеанс.Авторизация")]
    [Activity(Label = "@string/app_name")]
    public class AuthorizeActivity : BaseActivity<AuthenticateViewModel>
    {
        private string _selectedServer;

        public string SelectedServer
        {
            get => _selectedServer;
            set => this.RaiseAndSetIfChanged(ref _selectedServer, value);
        }

       

        [WireUpResource("activity_authorize_blur_view")]
        public BlurView BlurView { get; set; }


        [WireUpResource("version")]
        public TextView VersionText { get; set; }

        [HideOnResource("activity_authorize_et_barcode")]
        [WireUpResource("activity_authorize_et_barcode")]
        public CustomEditText BarCodeEditText { get; set; }

        [HideOnResource("activity_authorize_btn_entry")]
        [WireUpResource("activity_authorize_btn_entry")]
        public Button EntryButton { get; set; }

        [WireUpResource("activity_authorize_spinner_server")]
        public Spinner ServerSpinner { get; set; }

        [WireUpResource("activity_authorize_lottie_animation_view")]
        public LottieAnimationView LoadSpinner { get; set; }

        [WireUpResource("activity_authorize_tv_login_error")]
        public TextView DisplayErrorMessage { get; set; }

        public override void OnBackPressed()
        {
            Finish();
        }


        protected override void OnResume()
        {
            base.OnResume();

            //StopService(new Intent(this, typeof(KeepAliveService)));
            this.AttachSubscription<HideOnResourceAttribute, ConnectivityChangedEventArgs>(NetObservable, view =>
            {
                return args =>
                    view.Visibility = args.ConnectionProfiles.Any() ? ViewStates.Visible : ViewStates.Invisible;
            });
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //App.Stop(Application.Context);

            SetTheme(Resource.Style.AppThemeLouncher);

            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_login_authorize);
            if (ViewModel != null) ViewModel.BarCode = string.Empty;

            this.WireUpControls();

            this.RunOnceAction<HideOnResourceAttribute>(view =>
            {
                view.Visibility = Connectivity.ConnectionProfiles.Any() ? ViewStates.Visible : ViewStates.Invisible;
            });

            var drawable = Window?.DecorView;
            if (drawable != null) drawable.SystemUiVisibility = StatusBarVisibility.Hidden;
            var windowBackground = BaseLayout?.Background;

            BlurView.SetupWith(BaseLayout)
                .SetFrameClearDrawable(windowBackground)
                .SetBlurAlgorithm(new RenderScriptBlur(this))
                .SetBlurRadius(1.5f);
            BlurView.Visibility = ViewStates.Invisible;

            _configProvider = App.Container.Resolve<IConfigProvider>();
            var serviceList = _configProvider.BackendServices.Values.Select(c => c.ServiceName).ToArray();
            _serverAdapter = new CustomAdapter<string>(this, Resource.Layout.simple_spinner_item_white, serviceList, 0);
            ServerSpinner.Adapter = _serverAdapter;
            BarCodeEditText.Manager = Manager;
            BarCodeEditText.RequestFocus();
            VersionText.Text = "Сборка " + AppInfo.BuildString;
        }

        protected override void CommonSubscription(CompositeDisposable disposable)
        {
            this.OneWayBind(ViewModel,
                    vm => vm.BarCode,
                    v => v.BarCodeEditText.Text,
                    data => data)
                .DisposeWith(disposable);

            ScanReceiver.Scanned += (barcode) =>
            {
                BarCodeEditText.Text = barcode.BarcodeString;
                BarCodeEditText.DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Enter));
            };
        }

        protected override void ControlEventSubscription(CompositeDisposable disposable)
        {
            ServerSpinner.Events().ItemSelected
                .Do(_ => _configProvider.ChangeActiveService(ServerSpinner?.SelectedItem?.ToString()))
                .Subscribe(_ => InitializeCredentialToken());

            ServerSpinner.Events().ItemSelected
                .Subscribe(args =>
                {
                    _serverAdapter.SelectedPosition = args.Position;
                    SelectedServer = ServerSpinner?.SelectedItem?.ToString();
                })
                .DisposeWith(disposable);

            BarCodeEditText.Events().EditorAction
                .Where(args => (args.Event?.Action == KeyEventActions.Down & args.Event?.KeyCode == Keycode.Enter) | args.ActionId == ImeAction.Next)
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    if (string.IsNullOrWhiteSpace(BarCodeEditText.Text))
                    {
                        var toast = Toast.MakeText(this,
                            $"{GetString(Resource.String.empty_employee_field)}", ToastLength.Short);
                        toast?.Show();
                        return;
                    }
                    EntryButton.PerformClick();
                }).DisposeWith(disposable);

            EntryButton.Events().Click
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Do(_ =>
                {
                    if (BlurView != null) BlurView.Visibility = ViewStates.Visible;
                    LoadSpinner.PlayAnimation();
                    BaseLayout.Enabled = false;

                })
                .Select(_ => BarCodeEditText.Text)
                .InvokeCommand(this, x => x.ViewModel.Login)
                .DisposeWith(disposable);
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.Login
             .ObserveOn(HandlerScheduler.MainThreadScheduler)
             .Subscribe(ld =>
             {
                 if (!string.IsNullOrWhiteSpace(ViewModel.User.InfoMessageText))
                 {
                     var dialog = new OkSimpleDialog(this, ViewModel.User.InfoMessageText);
                     dialog.Show();
                     ViewModel.User.InfoMessageText = "";
                     dialog.Events().DismissEvent.Subscribe(_ =>
                     {
                         SharedDataManager.Add((User)ld);
                         if (!ld.ListEntities.IsNullOrEmpty())
                             SharedDataManager.AddList(ld.ListEntities);
                         if (ld.Entity != null)
                             SharedDataManager.Add(ld.Entity);
                         StartService(new Intent(this, typeof(KeepAliveService)));
                         StartActivity(ld.BusinessProcessState);
                         FinishAffinity();
                     }).DisposeWith(disposable);
                 }
                 else
                 {
                     SharedDataManager.Add((User)ld);
                     if (!ld.ListEntities.IsNullOrEmpty())
                         SharedDataManager.AddList(ld.ListEntities);
                     if (ld.Entity != null)
                         SharedDataManager.Add(ld.Entity);
                     StartService(new Intent(this, typeof(KeepAliveService)));
                     StartActivity(ld.BusinessProcessState);
                     FinishAffinity();
                 }
             })
             .DisposeWith(disposable);
        }

        protected override void ExceptionThrownSubscription(CompositeDisposable disposable)
        {
            ViewModel?.Login.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(e =>
                {
                    BarCodeEditText.Text = string.Empty;
                    var errorDialog = new ErrorDialog(this, e.Message);
                    errorDialog.Show();
                    LoadSpinner.CancelAnimation();
                    BlurView.Visibility = ViewStates.Invisible;
                })
                .DisposeWith(disposable);
        }

        protected void InitializeCredentialToken()
        {
            var credentials = App.Container.ResolveKeyed<NetworkCredential>("AxaptaCredentials");
            var credentialService = App.Container.Resolve<INetworkCredentials>();
            Task.Run(async () =>
            {
                var networkCredential = await credentialService.GetCredential();
                credentials.Domain = networkCredential.Domain;
                credentials.UserName = networkCredential.UserName;
                credentials.Password = networkCredential.Password;
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private IConfigProvider _configProvider;
        private CustomAdapter<string> _serverAdapter;
    }
}