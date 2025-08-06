using Android.App;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Autofac;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using TSD.PreviewDemo.App.Navigation;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.App.ViewData;
using TSD.PreviewDemo.App.ViewDialogs;
using TSD.PreviewDemo.Core.BarCodes;
using TSD.PreviewDemo.ViewModel;
using Xamarin.Essentials;

namespace TSD.PreviewDemo.App.Activities
{
    public class BaseActivity<T> : ScannerActivity<T> where T : BaseViewModel
    {
        private readonly ActivityNavigator _activityNavigator = ActivityNavigator.BuildNavigator(Assembly.GetExecutingAssembly());
        protected IObservable<ConnectivityChangedEventArgs> NetObservable;
        protected IObservable<ScannerBarcode> Scanner;

        public string Infomessage { get; set; }

        protected IActivitySharedDataManager SharedDataManager;

        protected ViewGroup Views;
        private bool _flagKeyBoard;
        protected InputMethodManager Manager;

        [WireUpResource("baseLayout")]
        public ConstraintLayout BaseLayout { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            App.Run(Application.Context);

            Infomessage = "";

            ViewModel = App.Container.Resolve<T>();
            var updateModelAction = App.Container.ResolveOptional<Action<T, IContainer>>();
            updateModelAction?.Invoke(ViewModel, App.Container);

            SharedDataManager = App.Container.Resolve<IActivitySharedDataManager>();

           Manager = (InputMethodManager) GetSystemService(InputMethodService);

            NetObservable =
                Observable.FromEvent<EventHandler<ConnectivityChangedEventArgs>, ConnectivityChangedEventArgs>(
                    handler =>
                    {
                        return (_, args) => handler(args);
                    },
                    h => Connectivity.ConnectivityChanged += h,
                    h => Connectivity.ConnectivityChanged -= h);

            Scanner =
                Observable.FromEvent<Action<ScannerBarcode>, ScannerBarcode>(
                    h => ScanReceiver.Scanned += h,
                    h => ScanReceiver.Scanned -= h);

            this.WhenActivated(disposable =>
            {
                CommonSubscription(disposable);
                ControlEventSubscription(disposable);
                CommandSubscription(disposable);
                ExceptionThrownSubscription(disposable);
            });
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (e is not { Action: KeyEventActions.Down } || e.KeyCode != Keycode.F1)
                return base.DispatchKeyEvent(e);
            var t = CurrentFocus;
            if (t == null) return true;
            if (t.GetType() != typeof(EditText) && t.GetType() != typeof(CustomEditText))
                return base.DispatchKeyEvent(e);
            if (_flagKeyBoard)
                Manager.HideSoftInputFromWindow(t.WindowToken, HideSoftInputFlags.None);
            else
                Manager.ShowSoftInput(t, ShowFlags.Implicit);

            _flagKeyBoard = !_flagKeyBoard;
            return base.DispatchKeyEvent(e);
        }

        protected override void OnResume()
        {
              
            this.WhenActivated(LoadData);
            base.OnResume();
            var t = CurrentFocus;
            if (t == null) return;
            if (t.GetType() != typeof(EditText)) return;
            if (_flagKeyBoard)
                Manager.HideSoftInputFromWindow(t.WindowToken, HideSoftInputFlags.None);
            else
                Manager.ShowSoftInput(t, ShowFlags.Implicit);
            _flagKeyBoard = !_flagKeyBoard;
        }

        protected override void OnPause()
        {
            ScanReceiver.RemoveAllSubscriptions();
            base.OnPause();
        }

        protected virtual void CommonSubscription(CompositeDisposable disposable) { }

        protected virtual void ControlEventSubscription(CompositeDisposable disposable) { }

        protected virtual void CommandSubscription(CompositeDisposable disposable) { }

        protected virtual void ExceptionThrownSubscription(CompositeDisposable disposable) { }

        protected virtual void LoadData(CompositeDisposable disposable) { }

        public void StartActivity(string businessProcessState)
        {
            var intent = _activityNavigator.NavigateTo(businessProcessState);
            StartActivity(intent);
        }
        protected void ShowInfoMessage()
        {
            if (!string.IsNullOrWhiteSpace(ViewModel?.User.InfoMessageText))
                new OkSimpleDialog(this, ViewModel.User.InfoMessageText).Show();
            if (ViewModel != null) ViewModel.User.InfoMessageText = "";
        }
    }
}
