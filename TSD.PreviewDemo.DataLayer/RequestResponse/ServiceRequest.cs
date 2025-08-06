using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Utils;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.DataEntities.Enums;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;

namespace TSD.PreviewDemo.DataLayer.RequestResponse
{
    public class ServiceRequest : IServiceRequest
    {
        // ReSharper disable UnusedMember.Global
        public string AppVersionNum { get; set; } = string.Empty;
        public YesNo BusinessProcessIsLocked { get; set; }
        public int CursorPosition { get; set; } = -1;
        public string DeviceId { get; set; } = string.Empty;
        public string PackedRequest { get; set; } = string.Empty;
        public string ServiceMethod { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;

        public static IServiceRequest Create(string serviceMethod, IEntity entity, YesNo businessProcessIsLocked = YesNo.No)
        {
            entity.ThrowIfNull(nameof(entity));
            entity.Session.ThrowIfNull("entity.Session");

            return new ServiceRequest
            {
                ServiceMethod = serviceMethod,
                BusinessProcessIsLocked = businessProcessIsLocked,
                DeviceId = entity.Session.DeviceId,
                SessionId = entity.Session.Id,
                AppVersionNum = entity.Session.AppVersionNum
            };
        }
        public static IServiceRequest Create<T>(string serviceMethod, T req, IEntity entity, YesNo businessProcessIsLocked = YesNo.No)
        {
            var baseReq = Create(serviceMethod, entity, businessProcessIsLocked);
            baseReq.PackedRequest = XmlSerializationHelpers.Serialize(req);
            return baseReq;
        }

        public static IServiceRequest Create<T>(string serviceMethod, T req, IServiceResponse serviceResponse, IEntity entity, YesNo businessProcessIsLocked = YesNo.No)
        {
            var baseReq = Create(serviceMethod, req, entity, businessProcessIsLocked);

            baseReq.PackedRequest = XmlSerializationHelpers.Serialize(req);
            baseReq.AppVersionNum = serviceResponse.AppVersionNum;
            baseReq.SessionId = serviceResponse.SessionId;

            return baseReq;
        }
    }
}