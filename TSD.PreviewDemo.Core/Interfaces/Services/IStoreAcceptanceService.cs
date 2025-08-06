using System.Threading.Tasks;
using TSD.PreviewDemo.Core.StoreAcceptance;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.Core.Interfaces.Services
{
    public interface IStoreAcceptanceService
    {
        Task<ProductScanData> PickJob(string request, User user);
        Task<ProductScanData> GetBarcodeScanningData(string barcode, User user);
        Task<JobList> CloseAcceptance(User user);
        Task<ProductScanData> SetItemInfo(ProductScanData productScanData, User user);
    }
}
