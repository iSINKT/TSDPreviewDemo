using System;
using TSD.PreviewDemo.DataEntities.Base;
using TSD.PreviewDemo.DataEntities.Enums;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.RequestResponse.Generic;

namespace TSD.PreviewDemo.DataLayer.RequestResponse
{
    public class ServiceResponse : IServiceResponse
    {
        public string AppVersionNum { get; set; }
        public string BusinessProcessCaption { get; set; }
        public string BusinessProcessState { get; set; }
        public int CursorPosition { get; set; }
        public DateTime DateTimeOfResponse { get; set; }
        public string ErrorCode { get; set; }
        public string PackedResponse { get; protected set; }
        public ResponseStatus ResponseStatus { get; set; }
        public string ServiceMethod { get; set; }
        public string SessionId { get; set; }
        public string TextMessage { get; set; }

        public bool InErrorState => (!string.IsNullOrWhiteSpace(ErrorCode) && !string.IsNullOrWhiteSpace(TextMessage)) || ResponseStatus == ResponseStatus.Error;

        public string Message { get; set; }

        public Error Error => InErrorState ? new Error(ErrorCode, TextMessage) : Error.Ok;

        public ServiceResponse()
        {

        }

        public ServiceResponse(string packedResponse)
        {
            PackedResponse = packedResponse;
        }

        public static IServiceResponse<T> Create<T>(IServiceResponse serviceResponse) where T: class
        {
            return new ServiceResponse<T>(serviceResponse);
        }
    }
}