using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using TSD.PreviewDemo.Common.Interceptors;
using TSD.PreviewDemo.Core.Utility;

namespace TSD.PreviewDemo.Core.Interfaces.Services.Utility
{
    [Intercept(typeof(AsyncInterceptorAdapter<LogInterceptor>))]
    public interface IUpdateApplicationService
    {
        Task<UpdateBundle> GetLastVersionBundle();
        Task<UpdateBundle> DownloadBundle(string path, UpdateBundle updateBundle);
    }
}