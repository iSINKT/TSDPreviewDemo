using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using ReactiveUI;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.StoreAcceptance;
using TSD.PreviewDemo.ViewModel.Menu;

namespace TSD.PreviewDemo.App.Activities.Menu
{
    [BusinessProcessState("Меню.ДвижениеТовара")]
    [Activity(Label = "MovementOfProductsMenuActivity", Theme = "@style/AppThemeWhiteToolbar")]
    // ReSharper disable once UnusedMember.Global
    public class MovementOfProductsMenuActivity : BaseActivity<MenuViewModel>
    {
        [WireUpResource("activity_menu_movement_of_product_btn_cancel")]
        public Button BtnCancel { get; set; }

        [WireUpResource("activity_menu_movement_of_product_btn_acceptance")]
        public Button BtnAcceptance { get; set; }

        [WireUpResource("activity_menu_movement_of_product_btn_cargo")]
        public Button BtnCargo { get; set; }

        [WireUpResource("activity_menu_movement_of_product_btn_returns")]
        public Button BtReturn { get; set; }

        [WireUpResource("activity_menu_movement_of_product_btn_materials")]
        public Button BtnSalesMaterials { get; set; }

        [WireUpResource("activity_menu_movement_of_product_tv_address_string")]
        public TextView AddressString { get; set; }

        [WireUpResource("activity_menu_movement_of_product_lottie_animation_view")]
        public LottieAnimationView LoadSpinnerAnim { get; set; }
        
        public override void OnBackPressed()
        {
            BtnCancel.PerformClick();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_menu_movement_of_product);

            this.WireUpControls();

            BtnCargo.Enabled = false;
            BtReturn.Enabled = false;
            BtnSalesMaterials.Enabled = false;
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
            BtnCancel.Events().Click
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                    if (ViewModel != null) ViewModel.NavigateTo = BusinessProcessState.Cancel;
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.Navigate)
                .DisposeWith(disposable);

            BtnAcceptance.Events().Click
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                    if (ViewModel != null) ViewModel.NavigateTo = BusinessProcessState.StoreAcceptance;
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.Navigate)
                .DisposeWith(disposable);
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.Navigate
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(entity =>
                {
                    if (entity.BusinessProcessState == "РегПрихода.ВыборЗаданийПоПриёмке")
                        SharedDataManager.Add((JobList)entity);

                    StartActivity(ViewModel?.User.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_right, Resource.Animation.slide_left_out);
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
        }
    }
}