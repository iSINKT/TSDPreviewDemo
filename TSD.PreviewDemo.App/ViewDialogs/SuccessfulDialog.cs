using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace TSD.PreviewDemo.App.ViewDialogs
{
    // ReSharper disable once UnusedMember.Global
    public class SuccessfulDialog : Dialog
    {
        public SuccessfulDialog(Activity activity) : base(activity)
        {
            Message = Context.GetString(Resource.String.dialog_tv_message);
        }

        public SuccessfulDialog(Activity activity, string message) : base(activity)
        {
            Message = message;
        }

        public override void OnBackPressed()
        {
            Confirm?.Invoke();
            base.OnBackPressed();
        }

        public event Action Confirm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature((int) WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_successful_message);

            var btnOk = (Button) FindViewById(Resource.Id.dialog_simple_successful_btn_ok);
            if (btnOk != null)
                btnOk.Click += (_, _) =>
                {
                    Confirm?.Invoke();
                    base.OnBackPressed();
                };


            var errorMessage = (TextView) FindViewById(Resource.Id.dialog_simple_successful_tv_message);
            if (errorMessage != null) errorMessage.Text = Message;
        }


        public override void Cancel()
        {
            Confirm?.Invoke();
            base.Cancel();
        }

        protected readonly string Message;
    }
}