using System;
using System.Net;
using Autofac;
using RestSharp;
using TSD.PreviewDemo.Client;
using TSD.PreviewDemo.Client.Wrappers;
using TSD.PreviewDemo.Common.Config;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.App.Container
{
    internal class BaseServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndpointAddressResolver>()
                .As<IEndpointAddressResolver>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IConfigProvider),
                    (_, ctx) => ctx.Resolve<IConfigProvider>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ServiceType),
                    (_, _) => ServiceType.AxaptaService)
                .Keyed<IEndpointAddressResolver>(ServiceType.AxaptaService)
                .SingleInstance();

            builder.RegisterType<EndpointAddressResolver>()
                .As<IEndpointAddressResolver>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IConfigProvider),
                    (_, ctx) => ctx.Resolve<IConfigProvider>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ServiceType),
                    (_, _) => ServiceType.UpdateRestService)
                .Keyed<IEndpointAddressResolver>(ServiceType.UpdateRestService)
                .SingleInstance();

            builder.RegisterType<AxCredentialsService>()
                .As<INetworkCredentials>()
                .As<IWarmAbleService>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ILogger),
                    (_, ctx) => ctx.Resolve<ILogger>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IRestClient),
                    (_, ctx) =>
                    {
                        var authUriService =
                            new Uri(ctx.Resolve<IConfigProvider>().ActiveBackendService.AxaptaAuthentication);
                        var restClient = new RestClient(authUriService);
                        return restClient;
                    });

            builder.RegisterType<AxWebService>()
                .As<IServiceExecutor<IServiceResponse, IServiceRequest>>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ILogger),
                    (_, ctx) => ctx.Resolve<ILogger>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(int),
                    (_, ctx) => ctx.Resolve<IConfigProvider>().WebRequestGlobalTimeout)
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(NetworkCredential),
                    (_, ctx) => ctx.ResolveKeyed<NetworkCredential>("AxaptaCredentials"))
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IEndpointAddressResolver),
                    (_, ctx) => ctx.ResolveKeyed<IEndpointAddressResolver>(ServiceType.AxaptaService))
                .SingleInstance();

            builder.RegisterType<RestClient>()
                .As<IRestClient>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(Uri),
                    (_, ctx) => new Uri(ctx.Resolve<IConfigProvider>().ActiveBackendService.UpdateBundleServiceConfiguration.Uri))
                .OnActivated(args =>
                    args.Instance.Timeout = Convert.ToInt32(args.Context.Resolve<IConfigProvider>().ActiveBackendService.UpdateBundleServiceConfiguration.Time))
                .SingleInstance();

            builder.RegisterType<RestClientDownloadWrapper>()
                .As<IRestClientDownload>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IRestClient),
                    (_, ctx) => ctx.Resolve<IRestClient>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ILogger),
                    (_, ctx) => ctx.Resolve<ILogger>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IEndpointAddressResolver),
                    (_, ctx) => ctx.ResolveKeyed<IEndpointAddressResolver>(ServiceType.UpdateRestService))
                .SingleInstance();

            base.Load(builder);
        }
    }
}