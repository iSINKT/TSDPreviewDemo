using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.CardView.Widget;
using Com.Airbnb.Lottie;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.Core.BarCodes;
using TSD.PreviewDemo.Core.StoreAcceptance;
using TSD.PreviewDemo.ViewModel.StoreAcceptance;

namespace TSD.PreviewDemo.App.Activities.StoreAcceptance
{
    [BusinessProcessState("РегПрихода.ВыборЗаданийПоПриёмке")]
    [Activity(Label = "StoreAcceptanceNewTasksActivity", Theme = "@style/AppThemeWhiteToolbar")]
    // ReSharper disable once UnusedMember.Global
    public class StoreAcceptanceNewTasksActivity : BaseActivity<StoreAcceptanceViewModel>
    {

        [WireUpResource("activity_store_acceptance_new_tasks_tv_address_string")]
        public TextView AddressString { get; set; }

        [WireUpResource("activity_store_acceptance_new_tasks_et_number")]
        public CustomEditText OrderNumberEditText { get; set; }

        [WireUpResource("activity_store_acceptance_new_tasks_et_cargo")]
        public CustomEditText CargoPackageBarcodeEditText { get; set; }

        [WireUpResource("activity_scan_item_tv_date")]
        public TextView DateTextView { get; set; }

        [WireUpResource("activity_store_acceptance_new_tasks_btn_cancel")]
        public Button BtnCancel { get; set; }

        [WireUpResource("activity_store_acceptance_return_confirmation_lottie_animation_view")]
        public LottieAnimationView LoadSpinnerAnim { get; set; }

        [WireUpResource("activity_store_acceptance_new_tasks_scroll_linear_layout")]
        public LinearLayout ScrollListDataView { get; set; }

        [WireUpResource("activity_store_acceptance_new_tasks_btn_refresh")]
        public AppCompatButton RefreshButton { get; set; }

        private List<CardView> _listOfCardview;

        public override void OnBackPressed()
        {
            BtnCancel.PerformClick();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_store_acceptance_new_tasks);

            this.WireUpControls();

            ShowInfoMessage();
            RefreshJournalList(ViewModel?.JobList);
        }
    
        protected override void LoadData(CompositeDisposable disposable)
        {
           RefreshButton.PerformClick();
            base.LoadData(disposable);
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
                })
                .Select(_ => Unit.Default)
                .InvokeCommand(ViewModel, vm => vm.BackNavigate)
                .DisposeWith(disposable);
           
            Scanner
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Where(barcode => barcode.BarcodeType != BarcodeType.Cargo)
                .Subscribe(barcode =>
                {
                    OrderNumberEditText.Text = barcode.BarcodeString;
                    CargoPackageBarcodeEditText.Text = string.Empty;
                    OrderNumberEditText.DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Enter));
                })
                .DisposeWith(disposable);

            Scanner
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Where(barcode => barcode.BarcodeType != BarcodeType.Item)
                .Subscribe(barcode =>
                {
                    CargoPackageBarcodeEditText.Text = barcode.BarcodeString;
                    OrderNumberEditText.Text = string.Empty;
                    CargoPackageBarcodeEditText.DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Enter));
                })
                .DisposeWith(disposable);
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.BackNavigate
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    
                    StartActivity(ViewModel?.User.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_left,
                        Resource.Animation.slide_right_out);
                    FinishAffinity();
                }).DisposeWith(disposable);

            ViewModel?.PickJob
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                   
                    StartActivity(ViewModel?.User.BusinessProcessState);
                    OverridePendingTransition(Resource.Animation.slide_right,
                        Resource.Animation.slide_left_out);
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

            ViewModel?.PickJob.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(exception =>
                {
                    LoadSpinnerAnim.Visibility = ViewStates.Invisible;
                    LoadSpinnerAnim.CancelAnimation();
                    new ErrorDialog(this, exception.Message).Show();
                })
                .DisposeWith(disposable);
        }

        private void RefreshJournalList(JobList jobList)
        {
            LoadSpinnerAnim.Visibility = ViewStates.Visible;
            LoadSpinnerAnim.PlayAnimation();

            DateTextView.Text = DateTime.Now.ToString("G");

            ScrollListDataView.RemoveAllViews();
            _listOfCardview = [];

            new Thread(() =>
            {
                foreach (var job in jobList.Jobs)
                {
                    var cardView = (CardView)LayoutInflater.From(this)?.Inflate(Resource.Layout.layout_store_acceptance_task_cardview, null);

                    var pieceByPiece = cardView?.FindViewById<TextView>(Resource.Id.layout_store_acceptance_task_cardview_piece_by_piece);
                    if (pieceByPiece != null) pieceByPiece.Text = job.Indicator ? "Ш" : string.Empty;

                    var jobNum = cardView?.FindViewById<TextView>(Resource.Id.layout_store_acceptance_task_cardview_order_number);
                    if (jobNum != null) jobNum.Text = job.JobNum == string.Empty ? "Пусто" : job.JobNum;

                    var cargoNumber = cardView?.FindViewById<TextView>(Resource.Id.layout_store_acceptance_task_cardview_cargo_number);
                    if (cargoNumber != null) cargoNumber.Text = job.InvoiceNum == string.Empty ? "Пусто" : job.InvoiceNum;

                    var jobType = cardView?.FindViewById<TextView>(Resource.Id.layout_store_acceptance_task_cardview_type_task);
                    if (jobType != null) jobType.Text = job.JobName == string.Empty ? "Пусто" : job.JobName;

                    var ordinalNumber = cardView?.FindViewById<TextView>(Resource.Id.layout_store_acceptance_task_cardview_ordinal_number);
                    if (ordinalNumber != null) ordinalNumber.Text = job.OrdinalNum;

                    var ttn = cardView?.FindViewById<TextView>(Resource.Id.layout_store_acceptance_task_cardview_ttn);
                    if (ttn != null) ttn.Text = job.AlcoType != string.Empty ? $"Тип НП {job.AlcoType}" : string.Empty;


                    if (cardView == null) continue;
                    {
                        cardView.Events().Click
                            .Do(_ =>
                            {
                                LoadSpinnerAnim.Visibility = ViewStates.Visible;
                                LoadSpinnerAnim.PlayAnimation();
                            })
                            .Select(_ =>
                            {
                                var jobItem = ViewModel?.JobList.Jobs.Single(jobItem => jobItem.OrdinalNum == ordinalNumber?.Text);
                                return jobItem?.JobId;
                            })
                            .InvokeCommand(ViewModel, v => v.PickJob);

                        RunOnUiThread(() =>
                        {
                            ScrollListDataView?.AddView(cardView);
                        });
                        _listOfCardview.Add(cardView);
                    }
                }

                var colors = ViewModel?.JobList.Colors;
                if (colors == null) return;
                foreach (var color in colors)
                {
                    var colorOfBackground = color.ObjectColor switch
                    {
                        16711680 => Color.ParseColor("#FF3D3D"),
                        16711660 => Color.ParseColor("#3DC5FF"),
                        16776960 => Color.ParseColor("#f0e84d"),
                        65535 => Color.ParseColor("#3df5f2"),
                        16744448 => Color.ParseColor("#ffbb00"),
                        _ => Color.White
                    };

                    RunOnUiThread(() =>
                    {
                        if (_listOfCardview.Count >= color.ObjectId)
                            _listOfCardview[color.ObjectId - 1].SetBackgroundColor(colorOfBackground);
                    });
                }
            }).Start();
            LoadSpinnerAnim.Visibility = ViewStates.Invisible;
            LoadSpinnerAnim.CancelAnimation();
        }
    }
}
