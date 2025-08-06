using Android.Content;
using RestSharp;
using TSD.PreviewDemo.DataLayer.UtilityServices;
using AndroidX.Work;
 using Autofac;
 using TSD.PreviewDemo.Common.Config;
 using TSD.PreviewDemo.Common.Platform;
// ReSharper disable UnusedVariable

 namespace TSD.PreviewDemo.App.BackgroundServices
{
    public class UploadLogsWorkManager(Context context, WorkerParameters workerParameters)
        : Worker(context, workerParameters)
    {
        public override Result DoWork()
        {
            var configProvider = App.Container.Resolve<IConfigProvider>();
            var deviceIdentityProvider = App.Container.Resolve<IDeviceIdentityProvider>();
            var restClient = new RestClient(configProvider.ActiveBackendService.UpdateBundleServiceConfiguration.Uri);

            var logService = new LogService(restClient, configProvider.LogFolder);
            return Result.InvokeSuccess();
        }
    }
}