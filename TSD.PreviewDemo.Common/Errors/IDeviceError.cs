// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global
namespace TSD.PreviewDemo.Common.Errors
{
    public interface IDeviceError
    {
        string ErrorMsg { get; set; }
        Error DeviceError { get; }
        IDeviceError GetErrorByCode(string errorCode, string errorMsg);
        void CleanError();
        void ShowInfoMessage(string msg);
        void ShowInfoMessage(string msg, string caption);
        bool ShowErrorMessage(IDeviceError error);
        bool ShowErrorMessage(IDeviceError error, string caption);
        void StopBackgroundRequests();
    }
}
