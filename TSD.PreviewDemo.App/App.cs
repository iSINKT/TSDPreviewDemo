using Android.Content;
using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using TSD.PreviewDemo.App.Container;
using TSD.PreviewDemo.App.Utilities;
using TSD.PreviewDemo.Common.App;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Common.Platform;
using TSD.PreviewDemo.Common.Utils;
// ReSharper disable ChangeFieldTypeToSystemThreadingLock

// ReSharper disable ConvertToAutoPropertyWithPrivateSetter
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.App
{
    public class App
    {
        public static void Run(Context context)
        {
            lock (Sync)
            {
                context.ThrowIfNull(nameof(context));
                if (IsStarted || _instance != null) return;

                var configurationRoot = BuildConfiguration(context);
                _instance = new App();
                _container = BuildContainer(context, configurationRoot);
                _isStarted = true;
                var logger = _container.Resolve<ILogger>();
                logger.Info("TSD application initialization completed.");
                logger.Info("TSD application started...");
            }
        }

        public static bool IsStarted => _isStarted;
        public static IContainer Container => _container;

        private static readonly object Sync = new();
        private static App _instance;
        private static IContainer _container;
        private static bool _isStarted;

        private App() { }

        private static IConfigurationRoot BuildConfiguration(Context context)
        {
            context.ThrowIfNull(nameof(context));

            if (context.Resources == null)
                throw new ApplicationException("Не найдены ресурсы приложения.");

            var inputStream = context.Resources.OpenRawResource(Resource.Raw.app_settings);
            string stringContent;

            using (var sr = new StreamReader(inputStream))
            {
                stringContent = sr.ReadToEnd();
            }

            return new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(stringContent)))
                .Build();
        }

        private static IContainer BuildContainer(Context context, IConfigurationRoot configurationRoot)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new ApplicationContextWrapper(context))
                .As<IApplicationContext>()
                .SingleInstance();
            builder.RegisterInstance(configurationRoot).As<IConfigurationRoot>().SingleInstance();
            builder.RegisterInstance(new AndroidDeviceIdentityProvider(context))
                .As<IDeviceIdentityProvider>()
                .SingleInstance();

            builder.RegisterModule<CommonModule>();
            builder.RegisterModule<BaseServicesModule>();
            builder.RegisterModule<CoreServicesModule>();
            builder.RegisterModule<ViewModelsModule>();
            
            _container = builder.Build();
            XmlSerializationHelpers.Init(Container?.Resolve<ILogger>());
            return _container;
        }
    }
}