using TSD.PreviewDemo.DataEntities.Enums;

namespace TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse
{
    public interface IServiceRequest
    {
        string AppVersionNum { get; set; }
        YesNo BusinessProcessIsLocked { get; set; }
        int CursorPosition { get; set; }
        string DeviceId { get; set; }
        string PackedRequest { get; set; }
        string ServiceMethod { get; set; }
        string SessionId { get; set; }
    }
}
