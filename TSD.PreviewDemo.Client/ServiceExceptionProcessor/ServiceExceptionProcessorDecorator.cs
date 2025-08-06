using System.Threading.Tasks;
using TSD.PreviewDemo.Client.Exceptions;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.DataEntities.Enums;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.RequestResponse;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace TSD.PreviewDemo.Client.ServiceExceptionProcessor
{
    public class ServiceExceptionProcessorDecorator(
        ILogger logger,
        IServiceExecutor<IServiceResponse, IServiceRequest> axWebService)
        : IServiceExecutor<IServiceResponse, IServiceRequest>
    {
        private readonly ILogger _logger = logger.ThrowIfNull(nameof(logger));
        private readonly IServiceExecutor<IServiceResponse, IServiceRequest> _service = axWebService.ThrowIfNull(nameof(axWebService));

        public IServiceResponse Execute(IServiceRequest request)
        {
            try
            {
                return _service.Execute(request);
            }
            catch (ServiceException ex)
            {
                return new ServiceResponse
                {
                    ResponseStatus = ResponseStatus.Error,
                    ErrorCode = ex.ExceptionCode,
                    TextMessage = ex.Message,
                };
            }
        }

        public Task<IServiceResponse> ExecuteAsync(IServiceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}