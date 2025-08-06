using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Castle.Core.Internal;
using Com.Airbnb.Lottie;
using ReactiveUI;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.Core.Users;
using TSD.PreviewDemo.ViewModel.Login;

// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.App.Activities.Login
{
    [BusinessProcessState("СменитьСклад.СписокСкладов")]
    [Activity(Label = "ChangeStorageActivity", Theme = "@style/AppThemeWhiteToolbar")]
    // ReSharper disable once UnusedMember.Global
    public class ChangeStorageActivity : BaseActivity<AuthenticateViewModel>
    {

        #region Prop
        [WireUpResource("activity_select_storage_tv_address_string")]
        public TextView AddressString { get; set; }

        [WireUpResource("activity_select_storage_btn_cancel")]
        public Button BtnCancel { get; set; }

        [WireUpResource("storage_spinner")]
        public Spinner StorageSpinner { get; set; }

        [WireUpResource("activity_select_storage_btn_create")]
        public Button BtnInput { get; set; }

        [WireUpResource("activity_select_storage_lottie_animation_view")]
        public LottieAnimationView LoadSpinnerAnim { get; set; }

        #endregion
        public override void OnBackPressed()
        {
            BtnCancel.PerformClick();
        }
        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (e.Action == KeyEventActions.Up && e.KeyCode != Keycode.Back) return false;
            return base.DispatchKeyEvent(e);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_select_storage);

            this.WireUpControls();
        }
        protected override void CommonSubscription(CompositeDisposable disposable)
        {
            this.OneWayBind(ViewModel,
                    vm => vm.User,
                    v => v.AddressString.Text,
                    data => data?.BusinessProcessCaption)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                   vm => vm.LocationChangeListData,
                   v => v._storageList,
                   data =>
                   {
                       _storageList = data?.LocationData;
                       if (!(_storageList != null & !_storageList.IsNullOrEmpty())) return _storageList;
                       var listInfo = (from dataStorage in ViewModel?.LocationChangeListData.LocationData select dataStorage.LocationId).ToList();
                       var filterAdapterReason = new CustomAdapter<string>(this,
                            Resource.Layout.simple_spinner_item_black_location,
                            listInfo, 0);
                       StorageSpinner.Adapter = filterAdapterReason;
                       return _storageList;
                   })
               .DisposeWith(disposable);
        }

        protected override void ControlEventSubscription(CompositeDisposable disposable)
        {
            BtnCancel.Events().Click
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.BackNavigate)
                .DisposeWith(disposable);

            BtnInput.Events().Click
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .Select(_ => StorageSpinner.SelectedItem?.ToString())
                .InvokeCommand(ViewModel, vm => vm.ChangeStorage)
                .DisposeWith(disposable);
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.BackNavigate
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    ShowInfoMessage();
                    StartActivity(ViewModel?.User.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_left,
                        Resource.Animation.slide_right_out);
                    FinishAffinity();
                }).DisposeWith(disposable);

            ViewModel?.ChangeStorage
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    if (!string.IsNullOrWhiteSpace(ViewModel.User.InfoMessageText))
                    {
                        var dialog = new OkSimpleDialog(this, ViewModel.User.InfoMessageText);
                        dialog.Show();

                        dialog.Events().KeyPress
                        .ObserveOn(HandlerScheduler.MainThreadScheduler)
                        .Where(args => args.Event is { Action: KeyEventActions.Down, KeyCode: Keycode.Enter })
                        .Subscribe(_ =>
                        {
                            if (ViewModel.User.BusinessProcessState != "СменитьСклад.СписокСкладов")
                            {
                                StartActivity(ViewModel.User.BusinessProcessState);
                                OverridePendingTransition(Resource.Animation.slide_left,
                                    Resource.Animation.slide_right_out);
                                FinishAffinity();
                            }
                            ViewModel.User.InfoMessageText = "";
                        })
                        .DisposeWith(disposable);
                    }
                    else
                   if (ViewModel.User.BusinessProcessState != "СменитьСклад.СписокСкладов")
                    {
                        StartActivity(ViewModel.User.BusinessProcessState);
                        OverridePendingTransition(Resource.Animation.slide_left,
                            Resource.Animation.slide_right_out);
                        FinishAffinity();
                    }
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                }).DisposeWith(disposable);
        }

        protected override void ExceptionThrownSubscription(CompositeDisposable disposable)
        {
            ViewModel?.BackNavigate.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);

            ViewModel?.ChangeStorage.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);
        }

        private List<ChangeListDataLocationData> _storageList;
    }
}
