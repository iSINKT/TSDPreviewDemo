using TSD.PreviewDemo.Common.Utils;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
// ReSharper disable PreferConcreteValueOverDefault

namespace TSD.PreviewDemo.DataLayer.RequestResponse.Generic
{
    public class ServiceResponse<T> : ServiceResponse, IServiceResponse<T> where T : class
    {
        public T Data { get; set; }

        public ServiceResponse(IServiceResponse serviceResponse)
        {
            AppVersionNum = serviceResponse.AppVersionNum;
            SessionId = serviceResponse.SessionId;
            ResponseStatus = serviceResponse.ResponseStatus;
            BusinessProcessCaption = serviceResponse.BusinessProcessCaption;
            PackedResponse = serviceResponse.PackedResponse;
            BusinessProcessState = serviceResponse.BusinessProcessState;
            CursorPosition = serviceResponse.CursorPosition;
            DateTimeOfResponse = serviceResponse.DateTimeOfResponse;
            ErrorCode = serviceResponse.ErrorCode;
            TextMessage = serviceResponse.TextMessage;
            ServiceMethod = serviceResponse.ServiceMethod;

            if (!string.IsNullOrWhiteSpace(PackedResponse) && !serviceResponse.InErrorState)
                Data = XmlSerializationHelpers.LoadData<T>(PackedResponse);
            else
            {
                Data = default(T);
            }
        }
    }
}