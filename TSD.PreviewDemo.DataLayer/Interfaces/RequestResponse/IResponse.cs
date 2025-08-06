using TSD.PreviewDemo.Common.Errors;

namespace TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse
{
    // ReSharper disable UnusedMember.Global
    public interface IResponse
    {
        IServiceResponse ServiceResponse { get; }
        Error Error { get; }
    }
}
