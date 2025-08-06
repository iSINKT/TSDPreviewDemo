using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.Shops;
using TSD.PreviewDemo.Core.Users;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.Services;
// ReSharper disable All
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace TSD.PreviewDemo.DataLayer.ServiceStubs
{
    public class UserServiceStub(
        ILogger logger,
        IServiceExecutor<IServiceResponse, IServiceRequest> serviceExecutor,
        IMapper mapper)
        : CoreService(logger, serviceExecutor, mapper), IUserService
    {
        public async Task<User> Init(string deviceId)
        {
            var user = new User
            {
                LoginState = LoginState.Initialized,
                BusinessProcessState = "Сеанс.НачальныеДанные"
            };
            return user;
        }

        Task IUserService.ChangeStorage(string storage, User user)
        {
            throw new NotImplementedException();
        }

        async Task<User> IUserService.CompleteInit(User user)
        {
            return new User
            {
                BusinessProcessState = "Сеанс.Авторизация",
                BusinessProcessCaption = "Авторизация по штрихкоду сотрудника"
            };
        }

        async Task<LoginData> IUserService.Login(User user)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(500);              
                return new LoginData()
                {
                    InfoMessageText = "Версия приложения для тестирования без подключения к аксапте. Обратитесь к разработчику приложения. \n\nStub version!",
                    BusinessProcessCaption = "Малахов А. > Выбор магазина и склада",
                    BusinessProcessState = "Сеанс.ВыборМагазинаИСклада",
                    Shops = new List<Shop>
                    {
                        new Shop
                        {
                            Name = "Объект 1",
                            Id = "Объект 1",
                            ShopLocations = new List<ShopLocation>
                            {
                                new ShopLocation()
                                {
                                    Id = "О_1_С_1",
                                    Name = "О_1_С_1"
                                }
                            }
                        },
                        new Shop
                        {
                            Name = "Объект 2",
                            Id = "Объект 2",
                            ShopLocations = new List<ShopLocation>
                            {
                                new ShopLocation()
                                {
                                    Id = "О_2_С_2",
                                    Name = "О_2_С_2"
                                }
                            }
                        },
                        new Shop
                        {
                            Name = "Объект 3",
                            Id = "Объект 3",
                            ShopLocations = new List<ShopLocation>
                            {
                                new ShopLocation()
                                {
                                    Id = "О_3_С_3",
                                    Name = "О_3_С_3"
                                }
                            }
                        },
                        new Shop
                        {
                            Name = "Объект 4",
                            Id = "Объект 4",
                            ShopLocations = new List<ShopLocation>
                            {
                                new ShopLocation()
                                {
                                    Id = "О_4_С_4",
                                    Name = "О_4_С_4"
                                }
                            }
                        },
                        new Shop
                        {
                            Name = "Объект 4",
                            Id = "Объект 4",
                            ShopLocations = new List<ShopLocation>
                            {
                                new ShopLocation()
                                {
                                    Id = "О_5_С_5",
                                    Name = "О_5_С_5"
                                }
                            }
                        }
                    },
                    LoginState = LoginState.SelectShop
                };
            });
        }

        async Task<User> IUserService.LoginCancel(User user)
        {
            throw new NotImplementedException();
        }

        async Task<User> IUserService.Logout(User user)
        {
            return new User
            {
                //InfoMessageText = string.Empty,
                BusinessProcessState = "Сеанс.Авторизация",
                BusinessProcessCaption = "Авторизация по штрихкоду сотрудника"
            };
        }
    }
}