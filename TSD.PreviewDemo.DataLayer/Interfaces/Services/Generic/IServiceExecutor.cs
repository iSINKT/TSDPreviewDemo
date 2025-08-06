using System.Threading.Tasks;
using RestSharp;

namespace TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic
{
    // ReSharper disable UnusedMember.Global
    public interface IServiceExecutor<out TResponse>
    {
        TResponse Execute();
    }

    public interface IServiceExecutor<TResponse, in TRequest>
    {
        TResponse Execute(TRequest request);
        Task<TResponse> ExecuteAsync(TRequest request);
    }

    public interface IServiceExecutorAsync<out TResponse, in TRequest>
    {
        TResponse Execute(TRequest request);
    }

    public interface IRestServiceExecutor : IServiceExecutor<IRestResponse, IRestRequest>
    {
        T Execute<T>(IRestRequest restRequest);
    }

    public interface IServiceDownloadExecutorAsync<in TRequest>
    {
        Task Download(TRequest request);
    }
}
