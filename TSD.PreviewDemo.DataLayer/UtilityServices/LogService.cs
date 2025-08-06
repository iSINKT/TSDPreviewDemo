using System;
using System.IO;
using System.Threading.Tasks;
using RestSharp;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Core.Interfaces.Services.Utility;

namespace TSD.PreviewDemo.DataLayer.UtilityServices
{
    public class LogService(IRestClient restClient, string logFolder) : ILogService
    {
        private readonly IRestClient _restClient = restClient.ThrowIfNull(nameof(restClient));
        private readonly string _logFolder = logFolder.ThrowIfNull(nameof(logFolder));

        public async Task UploadLogs(string deviceId)
        {
            var logFileArray = Directory.GetFiles(_logFolder, "*.log");

            if (logFileArray.Length > 0)
            {
                foreach (var logFile in logFileArray)
                {
                    var restRequest = new RestRequest($"api/log/upload/{deviceId}", Method.POST);
                    restRequest.AddFile("file", logFile);

                    var response = await _restClient.ExecuteAsync(restRequest);
                    if (response.ResponseStatus != ResponseStatus.Completed) continue;
                    var newName = logFile.Replace(".log", ".done");
                    try
                    {
                        File.Move(logFile, newName);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        public void DeleteLogs(string path, string format = "*.done")
        {
            var logFileArray = Directory.GetFiles(path, format);

            foreach (var logFile in logFileArray)
            {
                File.Delete(logFile);
            }
        }
    }
}
