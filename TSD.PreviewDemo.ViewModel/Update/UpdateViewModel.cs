using ReactiveUI;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Utils;
using TSD.PreviewDemo.Core.Interfaces.Services.Utility;
using TSD.PreviewDemo.Core.Utility;

namespace TSD.PreviewDemo.ViewModel.Update
{
    public class UpdateViewModel : BaseViewModel
    {
        public ReactiveCommand<UpdateRequest, UpdateBundle> Update { get; }
        public ReactiveCommand<(UpdateRequest, UpdateBundle), UpdateBundle> Download { get; }

        public UpdateViewModel(IUpdateApplicationService updateService)
        {
            updateService.ThrowIfNull(nameof(updateService));

            Update = ReactiveCommand.CreateFromTask<UpdateRequest, UpdateBundle>(async updateRequest =>
            {
                var latestBundle = await updateService.GetLastVersionBundle();
                latestBundle.UpdateState = string.Equals(latestBundle.InnerVersion, updateRequest.ActualAppVersion, StringComparison.InvariantCultureIgnoreCase) ? BundleState.UpdateNotRequired : BundleState.UpdateRequired;
                return latestBundle;

            }, Observable.Return(true), RxApp.TaskpoolScheduler);

            Download = ReactiveCommand.CreateFromTask<(UpdateRequest, UpdateBundle), UpdateBundle>(async updateRequest =>
            {
                var fullPath = Path.Combine(updateRequest.Item1.DownloadFolderPath, updateRequest.Item2.LocalBundleFilename);
                var flag = File.Exists(fullPath);
                if (!flag) 
                    return await updateService.DownloadBundle(fullPath, updateRequest.Item2);

                await Task.Run(() =>
                {
                    using (var fileStream = new FileStream(fullPath, FileMode.Open))
                    {
                        if (!string.Equals(fileStream.Sha256Hash(), updateRequest.Item2.Hash,
                            StringComparison.InvariantCultureIgnoreCase)) return;
                        updateRequest.Item2.UpdateState = BundleState.UpdateFromDiskRequired;
                        updateRequest.Item2.Path = fullPath;
                    }
                });

                if (updateRequest.Item2.UpdateState == BundleState.UpdateFromDiskRequired)
                    return updateRequest.Item2;
                return await updateService.DownloadBundle(fullPath, updateRequest.Item2);
            }, Observable.Return(true), RxApp.TaskpoolScheduler);
        }
    }
}