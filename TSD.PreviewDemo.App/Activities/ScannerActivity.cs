using Android.OS;
using Android.Runtime;
using ReactiveUI;
using System;
using TSD.PreviewDemo.App.BackgroundServices;

namespace TSD.PreviewDemo.App.Activities
{
    public class ScannerActivity : ReactiveActivity
    {
        protected static ScanReceiver ScanReceiver = new (Build.Manufacturer);

        protected ScannerActivity(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        public ScannerActivity()
        {
        }
    }

    public class ScannerActivity<TViewModel> : ScannerActivity, IViewFor<TViewModel>, ICanActivate 
        where TViewModel : class
    {
        private TViewModel _viewModel;

        protected ScannerActivity(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected ScannerActivity()
        {
        }

        public TViewModel ViewModel
        {
            get => _viewModel;
            set => this.RaiseAndSetIfChanged(ref _viewModel, value);
        }

        object IViewFor.ViewModel
        {
            get => _viewModel;
            set => _viewModel = (TViewModel)value;
        }
    }
}