using System;
using System.Globalization;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Com.Airbnb.Lottie;
using ReactiveUI;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.Core.BarCodes;
using TSD.PreviewDemo.Core.StoreAcceptance;
using TSD.PreviewDemo.ViewModel.StoreAcceptance;

// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.App.Activities.StoreAcceptance
{
    [BusinessProcessState("ПриемкаЗакупки.СканироватьШтрихкод")]
    [Activity(Label = "StoreAcceptanceProductScanActivity", Theme = "@style/AppThemeWhiteToolbar")]
    // ReSharper disable once UnusedMember.Global
    public class StoreAcceptanceProductScanActivity : BaseActivity<StoreAcceptanceViewModel>
    {

        #region Prop
        [WireUpResource("activity_store_acceptance_scan_product_tv_address_string")]
        public TextView AddressString { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_et_barcode")]
        public CustomEditText BarcodeEditText { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_tv_product_name")]
        public TextView ProductNameTextView { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_et_boxes")]
        public CustomEditText BoxesEditText { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_et_item_per_box")]
        public CustomEditText PerInBoxEditText { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_et_all_qty")]
        public CustomEditText AllQtyEditText { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_btn_inverse")]
        public AppCompatButton BtnInverse { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_btn_inverse_up")]
        public AppCompatButton BtnInverseUp { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_btn_dot")]
        public AppCompatButton BtnDot { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_et_fact_scanned")]
        public CustomEditText FactScannedEditText { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_et_date")]
        public CustomEditText DateEditText { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_cb_refuse")]
        public CheckBox CheckBoxReject { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_cb_accept")]
        public CheckBox CheckBoxAccept { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_btn_close")]
        public AppCompatButton BtnClose { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_btn_find")]
        public AppCompatButton BtnFindByMark { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_btn_cancel")]
        public Button BtnCancel { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_btn_input")]
        public Button BtnInput { get; set; }

        [WireUpResource("activity_store_acceptance_scan_product_lottie_animation_view")]
        public LottieAnimationView LoadSpinnerAnim { get; set; }
        public DatePickerDialog DatePickerDialog { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            BtnCancel.PerformClick();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_store_acceptance_scan_product);

            DatePickerDialog = new DatePickerDialog(this);

            this.WireUpControls();
            BtnFindByMark.Enabled = false;
        }
        protected override void CommonSubscription(CompositeDisposable disposable)
        {
            this.OneWayBind(ViewModel,
                    vm => vm.User,
                    v => v.AddressString.Text,
                    data => data?.BusinessProcessCaption)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.ProductNameTextView.Text,
                    data => data?.ItemName)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.BarcodeEditText.Enabled,
                    data => !data?.CheckMode)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.BtnClose.Enabled,
                    data => !data?.CheckMode)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.CheckBoxAccept.Enabled,
                    data => !data?.CheckMode)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.CheckBoxReject.Enabled,
                    data => !data?.CheckMode)
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.AllQtyEditText.Text,
                    data =>
                    {
                        if (data?.QuantityOfItemsPerBox != null)
                            AllQtyEditText.Text = ViewModel?.ProductScanData.UnitId == "КГ"
                                ? (data.QuantityOfItemsPerBox * data.QuantityOfBoxes).ToString("f3")
                                : (data.QuantityOfItemsPerBox * data.QuantityOfBoxes).ToString(CultureInfo.CurrentCulture);
                        return AllQtyEditText.Text;
                    })
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.FactScannedEditText.Text,
                    data =>
                    {
                        FactScannedEditText.Text = data?.QuantityFactScanned.ToString(CultureInfo.CurrentCulture);
                        if (ViewModel?.ProductScanData.UnitId == "КГ") FactScannedEditText.Text = data?.QuantityFactScanned.ToString("f3");
                        return FactScannedEditText.Text;
                    })
                .DisposeWith(disposable);

            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.BtnDot.Visibility,
                    data => data?.UnitId == "ШТ" ? ViewStates.Invisible : ViewStates.Visible)
                .DisposeWith(disposable);
           
            this.OneWayBind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.BoxesEditText.Enabled,
                    data => !data?.IsPieceMarkScheme)
                .DisposeWith(disposable);

            this.Bind(ViewModel,
                   vm => vm.ProductScanData,
                   v => v.DateEditText.Text,
                   data => DateEditText.Text = data?.ProductionDate.Year == 1900 ? "пусто" : data?.ProductionDate.ToString("dd/MM/yyyy"),
                   s =>
                   {
                       if (s == "пусто" || string.IsNullOrEmpty(ProductNameTextView.Text) || !DateTime.TryParse(s, out var date1))
                           return ViewModel?.ProductScanData;
                       if (ViewModel != null)
                           ViewModel.ProductScanData.ProductionDate = date1;
                       return ViewModel?.ProductScanData;
                   })
               .DisposeWith(disposable);

            this.Bind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.BoxesEditText.Text,
                    data => BoxesEditText.Text = data?.QuantityOfBoxes.ToString(CultureInfo.CurrentCulture),
                    s =>
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            if (s == "-") return ViewModel?.ProductScanData;
                            s = s.Replace(".", ",");
                            if (ViewModel == null) return ViewModel?.ProductScanData;
                            ViewModel.ProductScanData.QuantityOfBoxes = Convert.ToDecimal(s);
                        }
                        else
                        {
                            if (ViewModel != null) ViewModel.ProductScanData.QuantityOfBoxes = 0;
                        }
                        if (ViewModel != null)
                        {
                            AllQtyEditText.Text = (ViewModel.ProductScanData.QuantityOfItemsPerBox *
                                                   ViewModel.ProductScanData.QuantityOfBoxes)
                                .ToString(CultureInfo.CurrentCulture);
                        }
                        return ViewModel?.ProductScanData;
                    })
                .DisposeWith(disposable);

            this.Bind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.PerInBoxEditText.Text,
                    data =>
                    {
                        PerInBoxEditText.Text = data?.QuantityOfItemsPerBox.ToString(CultureInfo.CurrentCulture);
                        if (ViewModel?.ProductScanData.UnitId == "КГ") PerInBoxEditText.Text = data?.QuantityOfItemsPerBox.ToString("f3");
                        return PerInBoxEditText.Text;
                    },
                    s =>
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            if (s == "-") return ViewModel?.ProductScanData;
                            s = s.Replace(".", ",");
                            if (ViewModel == null) return ViewModel?.ProductScanData;
                            ViewModel.ProductScanData.QuantityOfItemsPerBox = Convert.ToDecimal(s);
                        }
                        else
                        {
                            if (ViewModel != null) ViewModel.ProductScanData.QuantityOfItemsPerBox = 0;
                        }
                        if (ViewModel?.ProductScanData.UnitId == "КГ")
                            AllQtyEditText.Text = (ViewModel.ProductScanData.QuantityOfItemsPerBox *
                                                   ViewModel.ProductScanData.QuantityOfBoxes)
                                .ToString("f3");
                        else if (ViewModel != null)
                            AllQtyEditText.Text = (ViewModel.ProductScanData.QuantityOfItemsPerBox *
                                                   ViewModel.ProductScanData.QuantityOfBoxes)
                                .ToString(CultureInfo.CurrentCulture);
                        return ViewModel?.ProductScanData;
                    })
                .DisposeWith(disposable);

            this.Bind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.BarcodeEditText.Text,
                    data => BarcodeEditText.Text = data?.ItemBarcode,
                    s =>
                    {
                        if (ViewModel != null)
                        {
                            ViewModel.ProductScanData.ItemBarcode = s;
                        }
                        return ViewModel?.ProductScanData;
                    })
                .DisposeWith(disposable);

            this.Bind(ViewModel,
                    vm => vm.ProductScanData,
                    v => v.CheckBoxReject.Checked,
                    data =>
                    {
                        return CheckBoxReject.Checked = data.Denial;
                    },
                    _ =>
                    {
                        if (ViewModel != null)
                        {
                            ViewModel.ProductScanData.Denial = CheckBoxReject.Checked;
                        }
                        return ViewModel?.ProductScanData;
                    })
                .DisposeWith(disposable);

            this.Bind(ViewModel,
                   vm => vm.ProductScanData,
                   v => v.CheckBoxAccept.Checked,
                   data =>
                   {
                       return CheckBoxAccept.Checked = !data.Denial;
                   },
                   _ =>
                   {
                       if (ViewModel != null)
                       {
                           ViewModel.ProductScanData.Denial = !CheckBoxAccept.Checked;
                       }
                       return ViewModel?.ProductScanData;
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

            Scanner
                .Where(barcode => barcode.BarcodeType == BarcodeType.Item)
                .Subscribe(barcode =>
                {
                    BarcodeEditText.Text = barcode.BarcodeString;
                    BarcodeEditText.DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Enter));
                })
                .DisposeWith(disposable);

            DateEditText.Events().TextChanged
                .Where((args, _) => args.AfterCount == 0)
                .Subscribe(_ =>
                {
                    DatePickerDialog.Show();
                })
                .DisposeWith(disposable);

            DateEditText.Events().FocusChange
                .Where((args, _) => !DatePickerDialog.IsShowing & string.IsNullOrWhiteSpace(DateEditText.Text) & args.HasFocus)
                .Subscribe(_ => DatePickerDialog.Show())
                .DisposeWith(disposable);

            DateEditText.Events().Click
                .Where((_, _) => !DatePickerDialog.IsShowing)
                .Subscribe(_ => DatePickerDialog.Show())
                .DisposeWith(disposable);

            DatePickerDialog.Events().DateSet
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(args =>
                {
                    if (ViewModel != null)
                        ViewModel.ProductScanData.ProductionDate =
                            new DateTime(args.Year, args.Month + 1, args.DayOfMonth);
                    DateEditText.RequestFocus();
                    DateEditText.SetSelection(DateEditText.Length());
                    DateEditText.SelectAll();
                })
                .DisposeWith(disposable);

            BtnInverse.Events().Click
                .Where(_ => !string.IsNullOrWhiteSpace(PerInBoxEditText.Text))
                .Subscribe(_ =>
                {
                    PerInBoxEditText.Text = PerInBoxEditText.Text != null && PerInBoxEditText.Text.StartsWith("-") ? PerInBoxEditText?.Text?.Replace("-", string.Empty) : PerInBoxEditText.Text?.Insert(0, "-");
                    PerInBoxEditText?.SetSelection(PerInBoxEditText.Length());
                })
                .DisposeWith(disposable);

            BtnDot.Events().Click
                .Where(_ => string.IsNullOrWhiteSpace(PerInBoxEditText.Text) || !PerInBoxEditText.Text.Contains(","))
                .Subscribe(_ =>
                {
                    PerInBoxEditText.Text += ",";
                    PerInBoxEditText.SetSelection(PerInBoxEditText.Length());
                })
                .DisposeWith(disposable);

            BtnInverseUp.Events().Click
                .Where(_ => !string.IsNullOrWhiteSpace(PerInBoxEditText.Text))
                .Subscribe(_ =>
                {
                    BoxesEditText.Text = BoxesEditText.Text != null && BoxesEditText.Text.StartsWith("-") ? BoxesEditText?.Text?.Replace("-", string.Empty) : BoxesEditText.Text?.Insert(0, "-");
                    BoxesEditText?.SetSelection(BoxesEditText.Length());
                })
                .DisposeWith(disposable);

            BtnClose.Events().Click                
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.CloseAcceptance)
                .DisposeWith(disposable);

            BarcodeEditText.Events().EditorAction
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Where(args => (args.Event?.Action == KeyEventActions.Down
                               && !string.IsNullOrEmpty(BarcodeEditText.Text)
                               && args.Event?.KeyCode == Keycode.Enter) |
                               args.ActionId == ImeAction.Next)
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .Select(_ => ViewModel?.ProductScanData.ItemBarcode)
                .InvokeCommand(ViewModel?.GetBarcodeScanningData)
                .DisposeWith(disposable);

            BoxesEditText.Events().EditorAction
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Where(args => args.Event is { Action: KeyEventActions.Down, KeyCode: Keycode.Enter }
                               | args.ActionId == ImeAction.Next)
                .Subscribe(_ =>
                {
                    PerInBoxEditText.RequestFocus();
                    PerInBoxEditText.SelectAll();
                })
                .DisposeWith(disposable);

            CheckBoxAccept.Events().CheckedChange
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Do(_ => { CheckBoxReject.Checked = !CheckBoxAccept.Checked; })
                .Subscribe()
                .DisposeWith(disposable);

            CheckBoxReject.Events().CheckedChange
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
               .Do(_ => { CheckBoxAccept.Checked = !CheckBoxReject.Checked; })
               .Subscribe()
               .DisposeWith(disposable);

            BtnInput.Events().Click
                .Where(_ => ViewModel != null && !ViewModel.ProductScanData.Denial)
                .Do(_ =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Visible;
                    LoadSpinnerAnim.PlayAnimation();
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.SetItemInfo)
                .DisposeWith(disposable);
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.BackNavigate
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    ShowInfoMessage();
                    ViewModel.ProductScanData = new ProductScanData();
                    StartActivity(ViewModel?.User.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_left,
                        Resource.Animation.slide_right_out);
                    FinishAffinity();
                }).DisposeWith(disposable);
            
            ViewModel?.GetBarcodeScanningData
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    ShowInfoMessage();
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                }).DisposeWith(disposable);

            ViewModel?.SetItemInfo
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    ShowInfoMessage();
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                }).DisposeWith(disposable);

            ViewModel?.CloseAcceptance
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    ShowInfoMessage();
                    ViewModel.ProductScanData = new ProductScanData();
                    StartActivity(ViewModel?.User.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_left,
                        Resource.Animation.slide_right_out);
                    FinishAffinity();
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

            ViewModel?.GetBarcodeScanningData.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);

            ViewModel?.SetItemInfo.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);

            ViewModel?.CloseAcceptance.ThrownExceptions
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