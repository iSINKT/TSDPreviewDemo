using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using TSD.PreviewDemo.Common.Interceptors;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.Core.Interfaces.Services
{
    // ReSharper disable UnusedMember.Global
    [Intercept(typeof(AsyncInterceptorAdapter<LogInterceptor>))]
    public interface IUserService
    {
        Task<User> Init(string deviceId);
        Task<User> CompleteInit(User user);
        Task<LoginData> Login(User user);
        Task<User> LoginCancel(User user);
        Task<User> Logout(User user);
        Task ChangeStorage(string storage, User user);
    }
}