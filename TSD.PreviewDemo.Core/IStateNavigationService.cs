using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using TSD.PreviewDemo.Common.Interceptors;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.Core
{
    [Intercept(typeof(AsyncInterceptorAdapter<LogInterceptor>))]
    public interface IStateNavigationService
    {
        Task<IEntity> SetState(User user, string state);
    }
}
