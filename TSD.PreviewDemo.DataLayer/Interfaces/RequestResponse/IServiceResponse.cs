using TSD.PreviewDemo.DataEntities.Base;
using TSD.PreviewDemo.DataEntities.Enums;

namespace TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse
{
    public interface IServiceResponse
    {
        string AppVersionNum { get; set; }
        string BusinessProcessCaption { get; set; }
        string BusinessProcessState { get; set; }
        int CursorPosition { get; set; }
        System.DateTime DateTimeOfResponse { get; set; }
        string ErrorCode { get; set; }
        string PackedResponse { get; }
        ResponseStatus ResponseStatus { get; set; }
        string ServiceMethod { get; set; }
        string SessionId { get; set; }
        string TextMessage { get; set; }
        bool InErrorState { get; }
        Error Error { get; }
    }

    public interface IServiceResponse<T> : IServiceResponse
    {
        T Data { get; set; }
    }
}