using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.StoreAcceptance;

namespace TSD.PreviewDemo.ViewModel.StoreAcceptance
{
    public class StoreAcceptanceViewModel : BaseViewModel
    {
        public ProductScanData ProductScanData
        {
            get => _productScanData;
            set => this.RaiseAndSetIfChanged(ref _productScanData, value);
        }
       
        public JobList JobList
        {
            get => _jobList;
            set => this.RaiseAndSetIfChanged(ref _jobList, value);
        }
       
        public ReactiveCommand<string, Unit> PickJob { get; }
        public ReactiveCommand<string, Unit> GetBarcodeScanningData { get; }
        public ReactiveCommand<Unit, Unit> BackNavigate { get; }
        public ReactiveCommand<Unit, Unit> CloseAcceptance { get; }
        public ReactiveCommand<Unit, Unit> SetItemInfo { get; }

        public StoreAcceptanceViewModel(IStoreAcceptanceService storeAcceptanceService, IStateNavigationService stateNavigationService)
        {
            storeAcceptanceService.ThrowIfNull(nameof(storeAcceptanceService));
            stateNavigationService.ThrowIfNull(nameof(stateNavigationService));

            BackNavigate = ReactiveCommand.CreateFromTask(async () =>
                {
                    var response = await stateNavigationService.SetState(User, "Cancel");
                    if (response.GetType() == JobList.GetType())
                    {
                        JobList = (JobList)response;
                    }                    
                    if (response.GetType() == ProductScanData.GetType())
                    {
                        ProductScanData = (ProductScanData)response;
                    }
                    
                },
                Observable.Return(true),
                RxApp.MainThreadScheduler);

            PickJob = ReactiveCommand.CreateFromTask<string>(async jobId =>
            {
                ProductScanData = await storeAcceptanceService.PickJob(jobId, User);
            },
                Observable.Return(true),
                RxApp.MainThreadScheduler);


            CloseAcceptance = ReactiveCommand.CreateFromTask(async () =>
            {
                JobList = await storeAcceptanceService.CloseAcceptance(User);
            },
                Observable.Return(true),
                RxApp.MainThreadScheduler);


            GetBarcodeScanningData = ReactiveCommand.CreateFromTask<string>(async barcode =>
            {
                ProductScanData = await storeAcceptanceService.GetBarcodeScanningData(barcode, User);
            },
                Observable.Return(true),
                RxApp.MainThreadScheduler);


            SetItemInfo = ReactiveCommand.CreateFromTask(async () =>
            {
                ProductScanData = await storeAcceptanceService.SetItemInfo(ProductScanData, User);
            },
                Observable.Return(true),
                RxApp.MainThreadScheduler);
        }

        private ProductScanData _productScanData;
        private JobList _jobList;
        
    }
}