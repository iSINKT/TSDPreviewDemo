using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TSD.PreviewDemo.App.ViewDialogs
{

    // ReSharper disable once UnusedMember.Global
    public class YesNoSimpleDialog : Dialog
    {
        public YesNoSimpleDialog(Context context) : base(context)
        {
            _message = Context.GetString(Resource.String.dialog_tv_message);
        }
        public YesNoSimpleDialog(Context context, string message) : base(context)
        {
            _message = message;
        }

        public override void OnBackPressed()
        {
            DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Escape));
            Cancel();
        }

        public event Action YesAction;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_yes_no_simple_dialog);

            var btnOk = (Button)FindViewById(Resource.Id.dialog_yes_no_simple_dialog_btn_ok);
            if (btnOk != null)
                btnOk.Click += (_, _) =>
                {
                    DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Enter));
                    YesAction?.Invoke();
                    Dismiss();
                };

            var btnCancel = (Button)FindViewById(Resource.Id.dialog_yes_no_simple_dialog_btn_cancel);
            if (btnCancel != null)
                btnCancel.Click += (_, _) => {
                    DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Escape));
                    Cancel(); };

            var errorMessage = (TextView)FindViewById(Resource.Id.dialog_yes_no_simple_dialog_tv_message);
            if (errorMessage != null) errorMessage.Text = _message;
        }

        private readonly string _message;
    }
}