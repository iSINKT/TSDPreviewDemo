// ReSharper disable once CheckNamespace
using TSD.PreviewDemo.DataEntities.Enums;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.RequestResponse;
// ReSharper disable CheckNamespace

namespace TSD.PreviewDemo.Client.AxWebServices
{
    // ReSharper disable once InconsistentNaming
    public partial class MCIServiceResponse
    {
        public static IServiceResponse Create(MCIServiceResponse request)
        {
            return new ServiceResponse(request.PackedResponse)
            {
                SessionId = request.SessionId,
                CursorPosition = request.CursorPosition,
                AppVersionNum = request.AppVersionNum,
                ServiceMethod = request.ServiceMethod,
                DateTimeOfResponse = request.DateTimeOfResponse,
                BusinessProcessState = request.BusinessProcessState,
                ResponseStatus = (ResponseStatus)(int)request.ResponseStatus,
                BusinessProcessCaption = request.BusinessProcessCaption,
                ErrorCode = request.ErrorCode,
                TextMessage = request.TextMessage
            };
        }
    }
}
