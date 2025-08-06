using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TSD.PreviewDemo.App.ViewDialogs
{

    // ReSharper disable once UnusedMember.Global
    public class YesNoSimpleWarningDialog : Dialog
    {
        public YesNoSimpleWarningDialog(Context context) : base(context)
        {
            _message = Context.GetString(Resource.String.dialog_tv_message);
        }
        public YesNoSimpleWarningDialog(Context context, string message) : base(context)
        {
            _message = message;
        }

        public override void OnBackPressed()
        {
            Cancel();
        }

        public Button BtnOk { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_yes_no_simple_warning);

            BtnOk = (Button)FindViewById(Resource.Id.dialog_yes_no_simple_dialog_btn_ok);

            var btnCancel = (Button)FindViewById(Resource.Id.dialog_yes_no_simple_dialog_btn_cancel);
            if (btnCancel != null)
                btnCancel.Click += (_, _) => { Cancel(); };

            var errorMessage = (TextView)FindViewById(Resource.Id.dialog_simple_warning_tv_message);
            if (errorMessage != null) errorMessage.Text = _message;
        }

        private readonly string _message;
    }
}







