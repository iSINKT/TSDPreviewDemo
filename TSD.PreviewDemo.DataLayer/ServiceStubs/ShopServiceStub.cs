using AutoMapper;
using System.Threading.Tasks;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.Users;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.Services;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace TSD.PreviewDemo.DataLayer.ServiceStubs
{
    public class ShopServiceStub(
        ILogger logger,
        IServiceExecutor<IServiceResponse, IServiceRequest> serviceExecutor,
        IMapper mapper)
        : CoreService(logger, serviceExecutor, mapper), IShopService
    {
        async Task<User> IShopService.SetShop(User user, string shop, string shopLocation)
        {
            return new User()
            {
                BusinessProcessCaption = "Малахов А. > Склад:02ГМ01_ОСН > Главное меню",
                BusinessProcessState = "Сеанс.ВыборМагазинаИСклада",
                LoginState = LoginState.SelectShop
            };
        }

        async Task<User> IShopService.CompleteSetShop(User user)
        {
            return new User()
            {
                BusinessProcessState = "Меню.Главный",
                BusinessProcessCaption = "Малахов А. > Склад:02ГМ01_ОСН > Главное меню",
                LoginState = LoginState.LoggedIn
            };
        }
    }
}
