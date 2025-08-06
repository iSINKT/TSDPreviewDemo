using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;

namespace TSD.PreviewDemo.DataLayer.Interfaces.Generic
{
    // ReSharper disable UnusedMember.Global
    public interface IResponse<out T> : IResponse where T : class
    {
        T Data { get; }
    }
}