using System.Threading.Tasks;

namespace TSD.PreviewDemo.Core.Interfaces.Services.Utility
{
    // ReSharper disable UnusedMember.Global
    public interface ILogService
    {
        Task UploadLogs(string deviceId);
        void DeleteLogs(string path, string format);
    }
}