using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TSD.PreviewDemo.App.Activities.Login;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.ViewModel.Menu;

// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.App.Activities.Menu
{
    [BusinessProcessState("Меню.Главный")]
    [Activity(Label = "MainMenuActivity", Theme = "@style/AppThemeWhiteToolbar")]
    // ReSharper disable once UnusedMember.Global
    public class MainMenuActivity : BaseActivity<MenuViewModel>
    {
        [WireUpResource("activity_main_menu_btn_inventory")]
        public Button BtnInventory { get; set; }

        [WireUpResource("activity_main_menu_btn_move_goods")]
        public Button BtnMoveGoods { get; set; }

        [WireUpResource("activity_main_menu_btn_my_tasks")]
        public Button BtnMyTasks { get; set; }

        [WireUpResource("activity_main_menu_btn_work_in_saleroom")]
        public Button BtnWorkInSaleroom { get; set; }

        [WireUpResource("activity_main_menu_btn_production")]
        public Button BtnProduction { get; set; }

        [WireUpResource("activity_main_menu_btn_cancel")]
        public Button BtnLogOut { get; set; }

        [WireUpResource("activity_main_menu_btn_change_storage")]
        public Button ChangeStorageBtn { get; set; }

        [WireUpResource("activity_main_menu_tv_address_string")]
        public TextView AddressString { get; set; }

        [WireUpResource("activity_main_menu_lottie_animation_view")]
        public LottieAnimationView LoadSpinnerAnim { get; set; }

        public override void OnBackPressed()
        {
            BtnLogOut.PerformClick();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main_menu);

            this.WireUpControls();

            BtnInventory.Enabled = false;
            BtnMyTasks.Enabled = false;
            BtnWorkInSaleroom.Enabled = false;
            BtnProduction.Enabled = false;
            ChangeStorageBtn.Enabled = false;
        }

        protected override void CommonSubscription(CompositeDisposable disposable)
        {
            this.OneWayBind(ViewModel,
                    vm => vm.User,
                    v => v.AddressString.Text,
                    data => data?.BusinessProcessCaption)
                .DisposeWith(disposable);
        }

        protected override void ControlEventSubscription(CompositeDisposable disposable)
        {
            BtnMoveGoods.Events().Click
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                    if (ViewModel != null) ViewModel.NavigateTo = BusinessProcessState.MenuMoveGoods;
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.Navigate)
                .DisposeWith(disposable);

            BtnLogOut.Events().Click
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.LogOut)
                .DisposeWith(disposable);
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.Navigate
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(entity =>
                {
                    StartActivity(entity.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_right, Resource.Animation.slide_left_out);
                    FinishAffinity();
                }).DisposeWith(disposable);

            ViewModel?.LogOut
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    StartActivity(new Intent(this,typeof(AuthorizeActivity)));
                    OverridePendingTransition(Resource.Animation.slide_left,
                        Resource.Animation.slide_right_out);
                    FinishAffinity();
                }).DisposeWith(disposable);
        }

        protected override void ExceptionThrownSubscription(CompositeDisposable disposable)
        {
            ViewModel?.Navigate.ThrownExceptions
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
    }
}