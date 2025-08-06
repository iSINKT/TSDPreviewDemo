using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TSD.PreviewDemo.App.ViewDialogs
{
    // ReSharper disable once UnusedMember.Global
    public class SimpleWarningDialog : Dialog
    {
        public SimpleWarningDialog(Activity activity) : base(activity)
        {
            _message = Context.GetString(Resource.String.dialog_tv_message);
        }

        public SimpleWarningDialog(Activity activity, string message) : base(activity)
        {
            _message = message;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_warning_message);

            var btnOk = (Button)FindViewById(Resource.Id.dialog_simple_warning_btn_ok);
            if (btnOk != null)
                btnOk.Click += (_, _) => { base.OnBackPressed(); };


            var errorMessage = (TextView)FindViewById(Resource.Id.dialog_simple_warning_tv_message);
            if (errorMessage != null) errorMessage.Text = _message;
        }

        private readonly string _message;
    }
}