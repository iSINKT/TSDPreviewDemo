using System.Threading.Tasks;
using RestSharp;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.Client.Wrappers
{
    public class RestClientDownloadWrapper : ServiceWrapper, IRestClientDownload
    {
        private readonly IRestClient _restClient;

        public RestClientDownloadWrapper(IRestClient restClient, ILogger logger, IEndpointAddressResolver endpointAddressResolver) : base(logger, endpointAddressResolver,
            ServiceType.UpdateRestService)
        {
            _restClient = restClient.ThrowIfNull(nameof(restClient));
            _restClient.BaseUrl = endpointAddressResolver.Resolve();
        }

        public async Task Execute(IRestRequest restRequest)
        {
            await Task.Run(() => CallServiceSync(() => _restClient.DownloadData(restRequest)));
        }
    }
}
