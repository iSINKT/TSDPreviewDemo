using AutoMapper;
using Newtonsoft.Json;
using RestSharp;
using System.IO;
using System.Threading.Tasks;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Core.Interfaces.Services.Utility;
using TSD.PreviewDemo.Core.Utility;
using TSD.PreviewDemo.DataEntities.Utilities;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.DataLayer.UtilityServices
{
    public class UpdateApplicationService(
        ILogger logger,
        IRestClient restClient,
        IRestClientDownload restClientDownload,
        IMapper mapper)
        : BaseService(logger, mapper), IUpdateApplicationService
    {
        private readonly IRestClient _restClient = restClient.ThrowIfNull(nameof(restClient));
        // ReSharper disable once UnusedMember.Local
        private readonly IRestClientDownload _restClientDownload = restClientDownload.ThrowIfNull(nameof(restClientDownload));

        public async Task<UpdateBundle> GetLastVersionBundle()
        {
            var request = new RestRequest("api/bundle/lastVersion/metadata", Method.GET);

            var tempClient = new RestClient(_restClient.BaseUrl?.ToString() ?? string.Empty)
            {
                Timeout = 5000
            };
            var response = await tempClient.ExecuteAsync<MCIRequestUpdateBundle>(request);

            if (!response.IsSuccessful)
            {
                CreateError(response.StatusCode.ToString(), response.ErrorMessage);
            }

            var lastBundleVersion = JsonConvert.DeserializeObject<MCIRequestUpdateBundle>(response.Content);
            var updateBundle = Mapper.Map<UpdateBundle>(lastBundleVersion);

            return updateBundle;
        }

        public async Task<UpdateBundle> DownloadBundle(string path, UpdateBundle updateBundle)
        {
            var directory = Path.GetDirectoryName(path);
            DeleteAllApk(directory);

            var writer = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            var request = new RestRequest($"api/bundle/context/{updateBundle.SoftwareBundleId}")
            {
                ResponseWriter = responseStream =>
                {
                    using (responseStream)
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        responseStream.CopyTo(writer);
                    }
                }
            };
            var restClientDownload = new RestClient(_restClient.BaseUrl?.ToString() ?? string.Empty);
            await restClientDownload.ExecuteAsync(request);
            writer.Close();

            updateBundle.Path = path;
            updateBundle.UpdateState = BundleState.UpdateRequired;
            return updateBundle;
        }

        protected BusinessError CreateError(string errorCode, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorCode))
                errorCode = "Error";
            throw new BusinessError(errorCode, errorMessage, string.Empty, string.Empty);
        }

        private void DeleteAllApk(string directory)
        {
            var files = Directory.GetFiles(directory, "*.apk");
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}
