using System.Net;
using System.Threading.Tasks;

namespace TSD.PreviewDemo.DataLayer.Interfaces.Services
{
    public interface INetworkCredentials
    {
        Task<NetworkCredential> GetCredential();
    }
}