using Autofac.Extras.DynamicProxy;
using System.Threading.Tasks;
using TSD.PreviewDemo.Common.Interceptors;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.Core.Interfaces.Services
{
    [Intercept(typeof(AsyncInterceptorAdapter<LogInterceptor>))]
    public interface IShopService
    {
        Task<User> SetShop(User user, string shop, string shopLocation);
        Task<User> CompleteSetShop(User user);
    }
}
