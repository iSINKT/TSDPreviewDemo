using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using TSD.PreviewDemo.Client.AxWebServices;
using TSD.PreviewDemo.Client.Wrappers;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.Client
{
    public class AxWebService(
        ILogger logger,
        int requestTimeout,
        NetworkCredential credentials,
        IEndpointAddressResolver endpointAddressResolver)
        : ServiceWrapper(logger, endpointAddressResolver, ServiceType.AxaptaService),
            IServiceExecutor<IServiceResponse, IServiceRequest>,
            IWarmAbleService
    {
        #region private fields

        // ReSharper disable once UnusedMember.Local
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly NetworkCredential _serviceCredentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

        #endregion

        public IServiceResponse Execute(IServiceRequest request)
        {
            var client = new MCIClient();
            client.Endpoint.Address = new EndpointAddress(EndpointAddressResolver.Resolve());
            client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, requestTimeout);
            client.ClientCredentials.UserName.UserName =
                $"{_serviceCredentials.Domain}\\{_serviceCredentials.UserName}";
            client.ClientCredentials.UserName.Password = _serviceCredentials.Password;

            return CallServiceSync(() =>
            {
                var response = client.reply(null, MCIServiceRequest.Create(request));
                ((IDisposable)client).Dispose();
                return MCIServiceResponse.Create(response);
            });
        }
        public async Task<IServiceResponse> ExecuteAsync(IServiceRequest request)
        {
            using var client = new MCIClient();
            client.Endpoint.Address = new EndpointAddress(EndpointAddressResolver.Resolve());
            client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, requestTimeout);
            client.ClientCredentials.UserName.UserName = $"{_serviceCredentials.Domain}\\{_serviceCredentials.UserName}";
            client.ClientCredentials.UserName.Password = _serviceCredentials.Password;

            return await CallService(() =>
            {
                var response = client.reply(null, MCIServiceRequest.Create(request));
                var createdResponse = MCIServiceResponse.Create(response);
                return createdResponse;
            });
        }

        public void WarmUp()
        {
            throw new NotImplementedException();
        }
    }
}
