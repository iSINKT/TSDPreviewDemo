using System;
using System.Threading.Tasks;
using AutoMapper;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.StoreAcceptance;
using TSD.PreviewDemo.Core.Users;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.Services;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.DataLayer.ServiceStubs
{
    public class StoreAcceptanceService(
        ILogger logger,
        IServiceExecutor<IServiceResponse, IServiceRequest> serviceExecutor,
        IMapper mapper)
        : CoreService(logger, serviceExecutor, mapper), IStoreAcceptanceService
    {
        public async Task<ProductScanData> PickJob(string request, User user)
        {
            user.BusinessProcessState = "ПриемкаЗакупки.СканироватьШтрихкод";
            user.BusinessProcessCaption = "ПриемкаЗакупки.СканироватьШтрихкод";
            var response = new ProductScanData();
            return response;
        }
        
        public async Task<JobList> CloseAcceptance(User user)
        {
            user.BusinessProcessState = "РегПрихода.ВыборЗаданийПоПриёмке";
            return new JobList()
            {
                BusinessProcessState = "РегПрихода.ВыборЗаданийПоПриёмке",
                BusinessProcessCaption = "Малахов А.Е. > ВыборЗаданийПоПриёмке",
                TopJobsForSelection = "20",
                Jobs =
                [
                    new Job()
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

                    new Job()
                    {
                        AllowPick = "Yes",
                        GateNum = "Не указан",
                        Indicator = false,
                        InvoiceNum = "",
                        JobId = "5638010077",
                        JobName = "Приемка",
                        JobNum = "01БЗК000013849",
                        OrdinalNum = "2"
                    },

                    new Job()
                    {
                        AllowPick = "Yes",
                        GateNum = "Не указан",
                        Indicator = false,
                        InvoiceNum = "",
                        JobId = "5638010078",
                        JobName = "Приемка",
                        JobNum = "01БЗК000013850",
                        OrdinalNum = "3"
                    },

                    new Job()
                    {
                        AllowPick = "Yes",
                        GateNum = "Не указан",
                        Indicator = false,
                        InvoiceNum = "",
                        JobId = "5638010826",
                        JobName = "Приемка",
                        JobNum = "01БЗК000013855",
                        OrdinalNum = "4"
                    }
                ]
            };
        }

        public async Task<ProductScanData> GetBarcodeScanningData(string barcode, User user)
        {
            var response = new ProductScanData
            {
                UnitId = "КГ",
                ItemBarcode = barcode,
                ItemName = $"Товарная позиция с номенклатурой {barcode}",
                ProductionDate = DateTime.MinValue
            };
            return response;
        }

        public async Task<ProductScanData> SetItemInfo(ProductScanData productScanData, User user)
        {
            var response = new ProductScanData();
            return response;
        }
    }
}
