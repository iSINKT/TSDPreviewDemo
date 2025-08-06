using System.Collections.Generic;
using Serilog;
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace TSD.PreviewDemo.Common.Config
{
    public interface IConfigProvider
    {
        string LogPath { get; }
        string LogFolder { get; }
        string ConfigPath { get; }
        string DownloadsDirectoryPath { get;  }
        string ServiceErrorPrefix { get; }

        int WebRequestGlobalTimeout { get; }

        IReadOnlyDictionary<string, BackendServicesConfiguration> BackendServices { get; }

        BackendServicesConfiguration ActiveBackendService { get; }

        LoggerConfiguration LoggerConfiguration { get;  }

        void ChangeActiveService(string activeServiceConfiguration);
    }

    public class UpdateBundleServiceConfiguration
    {
        public string Uri { get; set; }
        public string Time { get; set; }
    }

    public class BackendServicesConfiguration 
    {
        public string ServiceName { get; set; }
        public string AxaptaAuthentication { get; set; }
        public string AxaptaWeb { get; set; }
        public UpdateBundleServiceConfiguration UpdateBundleServiceConfiguration { get; set; }

        public override string ToString()
        {
            return ServiceName;
        }
    }
}
