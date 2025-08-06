using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.ViewModel.Login;
// ReSharper disable All

// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

namespace TSD.PreviewDemo.App.Activities.Login
{
    [BusinessProcessState("Сеанс.ВыборМагазинаИСклада")]
    [Activity(Label = "SelectShopActivity", Theme = "@style/AppThemeWhiteToolbar")]
    public class SelectShopActivity : BaseActivity<ShopViewModel>
    {
        private string _selectedShopId;
        private string _selectedStorageId;

        public string SelectedShopId
        {
            get => _selectedShopId;
            set => this.RaiseAndSetIfChanged(ref _selectedShopId, value);
        }

        public string SelectedStorageId
        {
            get => _selectedStorageId;
            set => this.RaiseAndSetIfChanged(ref _selectedStorageId, value);
        }

        [WireUpResource("activity_select_shop_btn_ok")]
        public Button BtnOk { get; set; }

        [WireUpResource("activity_select_shop_spinner_shop")]
        public Spinner ShopSpinner { get; set; }

        [WireUpResource("activity_select_shop_spinner_storage")]
        public Spinner StorageSpinner { get; set; }

        [WireUpResource("activity_select_shop_btn_exit")]
        public Button BtnLogOut { get; set; }

        [WireUpResource("activity_select_shop_lottie_animation_view")]
        public LottieAnimationView LoadSpinnerAnim { get; set; }

        public override void OnBackPressed()
        {
            BtnLogOut.PerformClick();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_select_shop);
            this.WireUpControls();

            BtnLogOut.Visibility = ViewStates.Gone;

            _customAdapter = new CustomAdapter<string>(this, Resource.Layout.simple_spinner_item_black,
                ViewModel?.User.Shops.Select(s => s.Id).ToList(), 0);
            ShopSpinner.Adapter = _customAdapter;

            this.WireUpControls();

            BtnOk.RequestFocus();
        }

        protected override void CommonSubscription(CompositeDisposable disposable)
        {
            this.Bind(ViewModel,
                    vm => vm.ShopId,
                    v => v.SelectedShopId,
                    data => SelectedShopId = data,
                    s => s)
                .DisposeWith(disposable);

            this.Bind(ViewModel,
                    vm => vm.StorageId,
                    v => v.SelectedStorageId,
                    data => SelectedStorageId = data,
                    s => s)
                .DisposeWith(disposable);
        }

        protected override void ControlEventSubscription(CompositeDisposable disposable)
        {
            ShopSpinner.Events().ItemSelected
                .Do(args =>
                {
                    ViewModel.ShopId = ShopSpinner?.SelectedItem?.ToString();
                    _customAdapter.SelectedPosition = args.Position;
                })
                .Select(args => Unit.Default)
                .InvokeCommand(this, v => v.ViewModel.SelectShop)
                .DisposeWith(disposable);

            BtnOk.Events()
                .Click
                .Select(args => Unit.Default)
                .Do(args =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .InvokeCommand(this, v => v.ViewModel.SetShopAndStorage)
                .DisposeWith(disposable);

            BtnLogOut.Events()
                .Click
                .Do(args =>
                {
                    if (LoadSpinnerAnim == null) return;
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .Select(args => Unit.Default)
                .InvokeCommand(this, v => v.ViewModel.LogOut)
                .DisposeWith(disposable);
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.LogOut
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(user =>
                {
                    StartActivity(new Intent(this, typeof(AuthorizeActivity)));
                    OverridePendingTransition(Resource.Animation.slide_left,
                        Resource.Animation.slide_right_out);
                    FinishAffinity();
                })
                .DisposeWith(disposable);

            ViewModel?.SetShopAndStorage
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(user =>
                {
                    StartActivity(ViewModel?.User.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_left,
                        Resource.Animation.slide_right_out);
                    FinishAffinity();
                });

            ViewModel?.SelectShop
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(shop =>
                {
                    var storageAdapter = new CustomAdapter<string>(this, Resource.Layout.simple_spinner_item_black,
                        shop.ShopLocations.Select(s => s.Id).ToList(), 0);
                    StorageSpinner.Adapter = storageAdapter;

                    StorageSpinner.ItemSelected += (s, e) =>
                    {
                        SelectedStorageId = StorageSpinner?.SelectedItem?.ToString();
                        storageAdapter.SelectedPosition = e.Position;
                    };
                })
                .DisposeWith(disposable);
        }

        protected override void ExceptionThrownSubscription(CompositeDisposable disposable)
        {
            ViewModel?.SelectShop.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);

            ViewModel?.SetShopAndStorage.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);

            ViewModel?.LogOut.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);
        }

        private CustomAdapter<string> _customAdapter;
    }
}