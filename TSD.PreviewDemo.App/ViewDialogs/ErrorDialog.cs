using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TSD.PreviewDemo.App.ViewDialogs
{
    public class ErrorDialog : Dialog
    {
        private readonly string _message;
        public ErrorDialog(Activity activity) : base(activity)
        {
            _message = Context.GetString(Resource.String.dialog_tv_message);
        }

        public ErrorDialog(Activity activity, string message) : base(activity)
        {
            _message = message;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_error_message);

            var btnOk = (Button)FindViewById(Resource.Id.dialog_error_message_btn_ok);
            if (btnOk != null)
                btnOk.Click += (_, _) => {
                    DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Enter));
                    Dismiss(); };

            var errorMessage = (TextView)FindViewById(Resource.Id.dialog_error_message_tv_message);
            if (errorMessage != null) errorMessage.Text = _message;
        }
    }
}