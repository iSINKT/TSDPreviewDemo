using AutoMapper;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;

namespace TSD.PreviewDemo.DataLayer.Services
{
    public abstract class BaseService(ILogger logger, IMapper mapper)
    {
        protected IMapper Mapper = mapper.ThrowIfNull(nameof(mapper));
        protected readonly ILogger Logger = logger.ThrowIfNull(nameof(logger));
    }
}