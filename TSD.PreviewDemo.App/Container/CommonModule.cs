using System;
using System.Linq;
using System.Net;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using TSD.PreviewDemo.App.ViewData;
using TSD.PreviewDemo.Common.App;
using TSD.PreviewDemo.Common.Config;
using TSD.PreviewDemo.Common.Interceptors;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Config;
using TSD.PreviewDemo.Logger;

namespace TSD.PreviewDemo.App.Container
{
    internal class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigProvider>()
                .As<IConfigProvider>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IConfiguration),
                    (_, ctx) => ctx.Resolve<IConfigurationRoot>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IApplicationContext),
                    (_, ctx) => ctx.Resolve<IApplicationContext>())
                .SingleInstance();
            
            builder.RegisterType<SerilogWrapper>()
                .As<ILogger>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IConfigProvider),
                    (_, ctx) => ctx.Resolve<IConfigProvider>())
                .SingleInstance();
            
            builder.RegisterType<ActivitySharedDataManager>()
                .As<IActivitySharedDataManager>()
                .SingleInstance();

            builder.RegisterType<NetworkCredential>()
                .Keyed<NetworkCredential>("AxaptaCredentials")
                .SingleInstance();

            builder.RegisterGeneric(typeof(AsyncInterceptorAdapter<>));

            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(myAssembly => myAssembly.GetName().Name == "TSD.PreviewDemo.DataLayer");
            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(assembly);
            }));

            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();

            builder.RegisterType<LogInterceptor>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(ILogger),
                    (_, ctx) => ctx.Resolve<ILogger>())
                .AsSelf();

            base.Load(builder);
        }
    }
}