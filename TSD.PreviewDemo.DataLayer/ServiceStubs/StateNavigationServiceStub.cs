using AutoMapper;
using System.Threading.Tasks;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.StoreAcceptance;
using TSD.PreviewDemo.Core.Users;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.DataLayer.ServiceStubs
{
    public class StateNavigationService(
        ILogger logger,
        IServiceExecutor<IServiceResponse, IServiceRequest> serviceExecutor,
        IMapper mapper)
        : CoreService(logger,
            serviceExecutor, mapper), IStateNavigationService
    {
        public async Task<IEntity> SetState(User user, string state)
        {
            return await Task.Run(() =>
            {
                switch (state)
                {
                    case "Cancel":
                        user.BusinessProcessState = "Меню.Главный";
                        user.BusinessProcessCaption = "Малахов А.Е. > Главный";
                        return new Entity()
                        {
                            BusinessProcessState = "Меню.Главный",
                            BusinessProcessCaption = "Малахов А.Е. > Главный"
                        };
                    case "Меню.ДвижениеТовара":
                        return new Entity()
                        {
                            BusinessProcessState = "Меню.ДвижениеТовара",
                            BusinessProcessCaption = "Малахов А.Е. > ДвижениеТовара"
                        };
                    case "Меню.ПриёмкаОтгрузка":
                        user.BusinessProcessState = "РегПрихода.ВыборЗаданийПоПриёмке";
                        return new JobList()
                        {
                            BusinessProcessState = "РегПрихода.ВыборЗаданийПоПриёмке",
                            BusinessProcessCaption = "Малахов А.Е. > ВыборЗаданийПоПриёмке",
                            TopJobsForSelection = "20",
                            Jobs =
                            [
                                new()
                                {
                                    AllowPick = "Yes",
                                    GateNum = "Не указан",
                                    Indicator = false,
                                    InvoiceNum = "",
                                    JobId = "5637903576",
                                    JobName = "",
                                    JobNum = "",
                                    OrdinalNum = "1"
                                },

                                new()
                                {
                                    AllowPick = "Yes",
                                    GateNum = "Не указан",
                                    Indicator = false,
                                    InvoiceNum = "",
                                    JobId = "5638010077",
                                    JobName = "Приемка",
                                    JobNum = "Заказ1",
                                    OrdinalNum = "2"
                                },

                                new()
                                {
                                    AllowPick = "Yes",
                                    GateNum = "Не указан",
                                    Indicator = true,
                                    InvoiceNum = "",
                                    JobId = "5638010078",
                                    JobName = "Приемка",
                                    JobNum = "Заказ2",
                                    OrdinalNum = "3"
                                },

                                new()
                                {
                                    AllowPick = "Yes",
                                    GateNum = "Не указан",
                                    Indicator = false,
                                    InvoiceNum = "",
                                    JobId = "5638010826",
                                    JobName = "Приемка",
                                    JobNum = "Заказ3",
                                    OrdinalNum = "4"
                                }
                            ]
                        };
                    default:
                        user.BusinessProcessState = "Меню.Главный";
                        return new Entity();
                }
            });
        }
    }
}