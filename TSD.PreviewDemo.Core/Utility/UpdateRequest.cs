namespace TSD.PreviewDemo.Core.Utility
{
    public class UpdateRequest
    {
        public string DownloadFolderPath { get; }
        public string ActualAppVersion { get; }

        public UpdateRequest(string downloadPath, string actualAppVersion)
        {
            DownloadFolderPath = downloadPath;
            ActualAppVersion = actualAppVersion;
        }
    }
}