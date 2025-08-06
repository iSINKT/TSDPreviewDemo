using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Serilog;
using TSD.PreviewDemo.Common.App;
using TSD.PreviewDemo.Common.Config;
// ReSharper disable CanSimplifyDictionaryLookupWithTryGetValue

// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.Config
{
    public class ConfigProvider : IConfigProvider
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        public ConfigProvider(IConfiguration configuration, IApplicationContext appContext)
        {
            var configList = configuration.GetSection("Application:Services:ServiceList").Get<List<BackendServicesConfiguration>>();
            var servicesSection = configuration.GetSection("Application:Services");
            var configUpdateServices = servicesSection.GetSection("ServiceList").GetChildren();
            var updateServices = configUpdateServices.Select(s => s.GetSection("UpdateBundleService").Get<UpdateBundleServiceConfiguration>()).ToArray();

            for (var i = 0; i <  configList.Count; i++)
                 configList[i].UpdateBundleServiceConfiguration = updateServices[i];

            BackendServices = new ReadOnlyDictionary<string, BackendServicesConfiguration>(
                configList.ToDictionary(l => l.ServiceName));

            ActiveBackendService =  BackendServices.First(pair => !string.IsNullOrEmpty(pair.Key)).Value;

            WebRequestGlobalTimeout = int.Parse(configuration["Application:Services:WebRequestGlobalTimeout"]);
            ServiceErrorPrefix = configuration["Application:Services:ServiceErrorPrefix"];

            var logFileName = configuration["Serilog:WriteTo:0:Args:path"];
            LogPath = Path.Combine(appContext.ExternalFilesDir, logFileName);
            LogFolder = appContext.ExternalFilesDir;
            configuration["Serilog:WriteTo:0:Args:path"] = LogPath;
            LoggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);
            DownloadsDirectoryPath = appContext.DownloadsDirectory;
        }

        public string LogPath { get; }
        public string LogFolder { get; }
        public string ConfigPath => throw new NotImplementedException("During config to android platform adaptation.");
        public string DownloadsDirectoryPath { get; }

        public string ServiceErrorPrefix { get; }

        public int WebRequestGlobalTimeout { get; }

        public IReadOnlyDictionary<string, BackendServicesConfiguration> BackendServices { get; }
        public BackendServicesConfiguration ActiveBackendService { get; private set; }

        public LoggerConfiguration LoggerConfiguration { get; }

        public void ChangeActiveService(string activeServiceConfiguration)
        {
            if (!BackendServices.ContainsKey(activeServiceConfiguration))
                throw new ApplicationException($"Конфигурация сервисов с именем {activeServiceConfiguration} не найдена.");
            ActiveBackendService = BackendServices[activeServiceConfiguration];
        }
    }
}