namespace TSD.PreviewDemo.Common.App
{
    public interface IApplicationContext
    {
        string ExternalFilesDir { get; }
        string DownloadsDirectory { get; }
    }
}