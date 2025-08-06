using System.Threading.Tasks;
using RestSharp;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;

namespace TSD.PreviewDemo.DataLayer.Interfaces.Services
{
    public interface IRestClientDownload : IServiceExecutorAsync<Task, IRestRequest>
    // ReSharper disable once RedundantTypeDeclarationBody
    {
        
    }
}