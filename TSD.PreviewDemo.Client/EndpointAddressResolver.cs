using System;
using TSD.PreviewDemo.Common.Config;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.Client
{
    public class EndpointAddressResolver(IConfigProvider configProvider, ServiceType serviceType)
        : IEndpointAddressResolver
    {
        private readonly IConfigProvider _configProvider = configProvider.ThrowIfNull(nameof(configProvider));


        Uri IEndpointAddressResolver.Resolve()
        {
            return serviceType switch
            {
                ServiceType.AxaptaService => new Uri(_configProvider.ActiveBackendService.AxaptaWeb),
                ServiceType.UpdateRestService => new Uri(_configProvider.ActiveBackendService.UpdateBundleServiceConfiguration
                    .Uri),
                _ => new Uri(string.Empty)
            };
        }

        void IEndpointAddressResolver.UpdateUriAxService(string newEndPOint)
        {
            _configProvider.ActiveBackendService.AxaptaWeb = newEndPOint;
        }
    }
}