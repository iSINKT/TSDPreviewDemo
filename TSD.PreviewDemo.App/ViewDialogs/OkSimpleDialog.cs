using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TSD.PreviewDemo.App.ViewDialogs
{
    public class OkSimpleDialog : Dialog
    {
        private readonly string _message;
        public OkSimpleDialog(Activity activity) : base(activity)
        {
            _message = Context.GetString(Resource.String.dialog_tv_message);
        }

        public OkSimpleDialog(Activity activity, string message) : base(activity)
        {
            _message = message;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_ok_simple_message);

            var btnOk = (Button)FindViewById(Resource.Id.dialog_ok_simple_message_btn_ok);
            if (btnOk != null)
                btnOk.Click += (_, _) =>
                {
                    DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Enter));
                    Dismiss();
                };

            var message = (TextView)FindViewById(Resource.Id.dialog_ok_simple_message_tv_message);
            if (message != null) message.Text = _message;
        }
    }
}