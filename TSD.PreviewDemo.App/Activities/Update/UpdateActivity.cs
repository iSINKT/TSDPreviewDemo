using System.Reactive.Disposables;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Com.Airbnb.Lottie;
using ReactiveUI;
using TSD.PreviewDemo.App.Activities.Login;
using TSD.PreviewDemo.Core.Utility;
using TSD.PreviewDemo.ViewModel.Update;
using System.Reactive.Linq;
using System;
using Java.Lang;
using Xamarin.Essentials;
using FileProvider = AndroidX.Core.Content.FileProvider;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Autofac;
using TSD.PreviewDemo.Common.Config;
using Java.Util.Concurrent;
using TSD.PreviewDemo.App.BackgroundServices;
using Com.EightbitLab.BlurViewBinding;
using AndroidX.Work;
// ReSharper disable UnusedMember.Global

namespace TSD.PreviewDemo.App.Activities.Update
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class UpdateActivity : BaseActivity<UpdateViewModel>
    {
        [WireUpResource("activity_update_main_view")]
        public BlurView MainUpdateLayout { get; set; }

        [WireUpResource("activity_update_check_text")]
        public TextView UpdateText { get; set; }

        [WireUpResource("activity_update_lottie_animation_view")]
        public LottieAnimationView LoadSpinner { get; set; }

        [WireUpResource("baseLayout")]
        public ConstraintLayout UpdateFailedLayout { get; set; }

        [WireUpResource("activity_update_button_repeat")]
        public Button RepeatUpdateButton { get; set; }

        [WireUpResource("activity_update_button_continue_without_update")]
        public Button ContinueWithoutUpdate { get; set; }

        public bool IsCheckedForUpdate { get; set; }
        private UpdateRequest _updateRequest;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode != Result.Canceled) return;
            var intent = new Intent(Application.Context, typeof(AuthorizeActivity));
            StartActivity(intent);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.AppThemeLouncher);

            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_update);

            this.WireUpControls();

            var configProvider = App.Container.Resolve<IConfigProvider>();

            var uploadLogsRequest = PeriodicWorkRequest.Builder.From<UploadLogsWorkManager>(15, TimeUnit.Minutes).Build();
            WorkManager.GetInstance(Application.Context).EnqueueUniquePeriodicWork("upload_logs", ExistingPeriodicWorkPolicy.Replace, uploadLogsRequest);

            /*LoadSpinner.Visibility = ViewStates.Visible;
            LoadSpinner.PlayAnimation();*/

            //UpdateFailedLayout.Visibility = ViewStates.Invisible;

            _updateRequest = new UpdateRequest(
                configProvider.DownloadsDirectoryPath,
                AppInfo.BuildString);

        }

        protected override void OnResume()
        {
            base.OnResume();
            RepeatUpdateButton.PerformClick();
        }

        protected override void ControlEventSubscription(CompositeDisposable disposable)
        {
            RepeatUpdateButton.Events().Click
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Do(_ =>
                {
                    LoadSpinner.Visibility = ViewStates.Visible;
                    LoadSpinner.PlayAnimation();
                    UpdateText.Text = "Проверка обновления";
                    IsCheckedForUpdate = false;
                    RepeatUpdateButton.Visibility = ViewStates.Invisible;
                    ContinueWithoutUpdate.Visibility = ViewStates.Invisible;
                })
                .Select(_ => _updateRequest)
                .InvokeCommand(this, x => x.ViewModel.Update)
                .DisposeWith(disposable);

            ContinueWithoutUpdate.Click += (_, _) =>
            {
                var intent = new Intent(Application.Context, typeof(AuthorizeActivity));
                StartActivity(intent);
            };
        }

        protected override void CommandSubscription(CompositeDisposable disposable)
        {
            ViewModel?.Update
            .ObserveOn(HandlerScheduler.MainThreadScheduler)
            .Where(ld => ld.UpdateState == BundleState.UpdateRequired)
            .Subscribe(ld =>
            {

                UpdateText.Text = "Загрузка обновления";

                ViewModel?.Download
            .Execute((_updateRequest, ld))
            .ObserveOn(HandlerScheduler.MainThreadScheduler)
            .Subscribe(bundle =>
            {
                var apkToInstall = new Java.IO.File(bundle.Path);
                // ReSharper disable once StringLiteralTypo
                var authString = $"{Application.Context.PackageName}.fileprovider";
                var apkUri = FileProvider.GetUriForFile(Application.Context, authString, apkToInstall);

                var updateIntent = new Intent(Intent.ActionView);
                updateIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                updateIntent.PutExtra(Intent.ExtraReturnResult, true);
                updateIntent.SetDataAndType(apkUri, "application/vnd.android.package-archive");

                IsCheckedForUpdate = true;
                StartActivityForResult(updateIntent, 0);
            }).DisposeWith(disposable);

            })
            .DisposeWith(disposable);

            ViewModel?.Update
            .ObserveOn(HandlerScheduler.MainThreadScheduler)
            .Where(ld => ld.UpdateState == BundleState.UpdateNotRequired)
            .Subscribe(_ =>
            {
                var intent = new Intent(Application.Context, typeof(AuthorizeActivity));
                Thread.Sleep(500);
                StartActivity(intent);

            })
            .DisposeWith(disposable);
        }

        protected override void ExceptionThrownSubscription(CompositeDisposable disposable)
        {
            ViewModel?.Update.ThrownExceptions
                .ObserveOn(HandlerScheduler.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    UpdateText.Text = "Сервер недоступен";
                    LoadSpinner.Visibility = ViewStates.Invisible;
                    RepeatUpdateButton.Enabled = true;
                    RepeatUpdateButton.Visibility = ViewStates.Visible;
                    ContinueWithoutUpdate.Visibility = ViewStates.Visible;
                })
                .DisposeWith(disposable);
        }
        protected override void OnRestart()
        {
            base.OnRestart();
            Finish();
        }
    }
}