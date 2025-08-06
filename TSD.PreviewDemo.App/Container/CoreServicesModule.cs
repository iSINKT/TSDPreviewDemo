using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using RestSharp;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.Interfaces.Services.Utility;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.ServiceStubs;
using TSD.PreviewDemo.DataLayer.UtilityServices;

namespace TSD.PreviewDemo.App.Container
{
    internal sealed class CoreServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {           

            builder.RegisterType<UserServiceStub>()
                .As<IUserService>()
                .EnableInterfaceInterceptors()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ILogger),
                    (_, ctx) => ctx.Resolve<ILogger>())
                .WithParameter(
                    (info, _) =>
                        info.ParameterType == typeof(IServiceExecutor<IServiceResponse, IServiceRequest>),
                    (_, ctx) => ctx.Resolve<IServiceExecutor<IServiceResponse, IServiceRequest>>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IMapper),
                    (_, ctx) => ctx.Resolve<IMapper>())
                .SingleInstance();

            builder.RegisterType<ShopServiceStub>()
                .As<IShopService>()
                .EnableInterfaceInterceptors()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ILogger),
                    (_, ctx) => ctx.Resolve<ILogger>())
                .WithParameter(
                    (info, _) =>
                        info.ParameterType == typeof(IServiceExecutor<IServiceResponse, IServiceRequest>),
                    (_, ctx) => ctx.Resolve<IServiceExecutor<IServiceResponse, IServiceRequest>>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IMapper),
                    (_, ctx) => ctx.Resolve<IMapper>())
                .SingleInstance();

            builder.RegisterType<StateNavigationService>()
               .As<IStateNavigationService>()
               .EnableInterfaceInterceptors()
               .WithParameter(
                   (info, _) => info.ParameterType == typeof(ILogger),
                   (_, ctx) => ctx.Resolve<ILogger>())
               .WithParameter(
                   (info, _) =>
                       info.ParameterType == typeof(IServiceExecutor<IServiceResponse, IServiceRequest>),
                   (_, ctx) => ctx.Resolve<IServiceExecutor<IServiceResponse, IServiceRequest>>())
               .WithParameter(
                   (info, _) => info.ParameterType == typeof(IMapper),
                   (_, ctx) => ctx.Resolve<IMapper>())
               .SingleInstance();

            builder.RegisterType<StoreAcceptanceService>()
               .As<IStoreAcceptanceService>()
               .EnableInterfaceInterceptors()
               .WithParameter(
                   (info, _) => info.ParameterType == typeof(ILogger),
                   (_, ctx) => ctx.Resolve<ILogger>())
               .WithParameter(
                   (info, _) =>
                       info.ParameterType == typeof(IServiceExecutor<IServiceResponse, IServiceRequest>),
                   (_, ctx) => ctx.Resolve<IServiceExecutor<IServiceResponse, IServiceRequest>>())
               .WithParameter(
                   (info, _) => info.ParameterType == typeof(IMapper),
                   (_, ctx) => ctx.Resolve<IMapper>())
               .SingleInstance();

            builder.RegisterType<UpdateApplicationService>()
                .As<IUpdateApplicationService>()
                .EnableInterfaceInterceptors()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ILogger),
                    (_, ctx) => ctx.Resolve<ILogger>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IRestClient),
                    (_, ctx) => ctx.Resolve<IRestClient>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IRestClientDownload),
                    (_, ctx) => ctx.Resolve<IRestClientDownload>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IMapper),
                    (_, ctx) => ctx.Resolve<IMapper>())
                .SingleInstance();
        }
    }
}