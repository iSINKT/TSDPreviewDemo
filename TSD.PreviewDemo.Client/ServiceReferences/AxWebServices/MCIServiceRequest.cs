// ReSharper disable once CheckNamespace
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
// ReSharper disable CheckNamespace

namespace TSD.PreviewDemo.Client.AxWebServices
{
    // ReSharper disable once InconsistentNaming
    public partial class MCIServiceRequest
    {
        public static MCIServiceRequest Create(IServiceRequest request)
        {
            return new MCIServiceRequest()
            {
                DeviceId = request.DeviceId,
                SessionId =  request.SessionId,
                CursorPosition = request.CursorPosition,
                AppVersionNum = request.AppVersionNum,
                ServiceMethod = request.ServiceMethod,
                BusinessProcessIsLocked = (NoYes)(int)request.BusinessProcessIsLocked,
                PackedRequest = request.PackedRequest
            };
        }
    }
}
