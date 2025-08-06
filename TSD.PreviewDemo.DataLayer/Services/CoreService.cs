using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Common.Utils;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.DataEntities.Base;
using TSD.PreviewDemo.DataLayer.Interfaces.RequestResponse;
using TSD.PreviewDemo.DataLayer.Interfaces.Services.Generic;
using TSD.PreviewDemo.DataLayer.RequestResponse;
// ReSharper disable RedundantTypeDeclarationBody
// ReSharper disable UnusedMember.Global

namespace TSD.PreviewDemo.DataLayer.Services
{
    internal class ResponseStub { }
    internal class RequestStub { }

    public class CoreService : BaseService
    {
        private readonly IServiceExecutor<IServiceResponse, IServiceRequest> _service;

        protected CoreService(ILogger logger, IServiceExecutor<IServiceResponse, IServiceRequest> serviceExecutor, IMapper mapper) : base(logger, mapper)
        {
            _service = serviceExecutor.ThrowIfNull(nameof(serviceExecutor));
        }

        #region sync methods
        
        protected IServiceResponse<TResponse> Call<TResponse, TRequest>(IEntity entity, TRequest request, string serviceMethod) where TResponse : class
        {
            return (IServiceResponse<TResponse>)CommonCall<TResponse, TRequest>(entity, request, serviceMethod).Result;
        }

        protected IServiceResponse<TResponse> Call<TResponse>(IEntity entity, string serviceMethod) where TResponse : class
        {
            return (IServiceResponse<TResponse>)CommonCall<TResponse, RequestStub>(entity, null, serviceMethod).Result;
        }

        protected IServiceResponse Call(IEntity entity, string serviceMethod) 
        {
            return CommonCall<ResponseStub, RequestStub>(entity, null, serviceMethod).Result;
        }

        protected IServiceResponse Call<TRequest>(IEntity entity, TRequest request, string serviceMethod)
        {
            return CommonCall<ResponseStub, TRequest>(entity, request, serviceMethod).Result;
        }

        #endregion

        #region async methods

        protected async Task<IServiceResponse<TResponse>> CallAsync<TResponse, TRequest>(IEntity entity, TRequest request, string serviceMethod) where TResponse : class
        {
            var response = await CommonCall<TResponse, TRequest>(entity, request, serviceMethod);
          
            return (IServiceResponse<TResponse>) response;
        }

        protected async Task<IServiceResponse<TResponse>> CallAsync<TResponse>(IEntity entity, string serviceMethod) where TResponse : class
        {
            var response = await CommonCall<TResponse, RequestStub>(entity, null, serviceMethod);
         
            return (IServiceResponse<TResponse>)response;
        }

        protected async Task<IServiceResponse> CallAsync(IEntity entity, string serviceMethod)
        {
            var response = await CommonCall<ResponseStub, RequestStub>(entity, null, serviceMethod);
            
            return response;
        }

        protected async Task<IServiceResponse> CallAsync<TRequest>(IEntity entity, TRequest request, string serviceMethod)
        {
            var response = await CommonCall<ResponseStub, TRequest>(entity, request, serviceMethod);
          
            return response;
        }

        #endregion

        protected BusinessError CreateError(IServiceResponse serviceResponse)
        {
            serviceResponse.ThrowIfNull(nameof(serviceResponse));
            if (!serviceResponse.InErrorState) return BusinessError.Ok;

            var errCode = serviceResponse.Error.Code;
            var errMsg = serviceResponse.Error.Message;
            var state = serviceResponse.BusinessProcessState;
            var caption = serviceResponse.BusinessProcessCaption;

            if (string.IsNullOrWhiteSpace(errCode))
                errCode = "Error";
            throw new BusinessError(errCode, errMsg, state, caption);
        }

        protected IEntity ParseNotSpecifiedResponse(IServiceResponse genericResponse)
        {
            genericResponse.ThrowIfNull(nameof(genericResponse));

            var respXml = XElement.Parse(genericResponse.PackedResponse);
            var rootElement = respXml.Name.LocalName;

            var dtoTypes = Assembly.GetAssembly(typeof(Error)).GetTypeListMarkedAs<XmlRootAttribute>();
            var dtoType = dtoTypes.FirstOrDefault(t => t.Name == respXml.Name.LocalName);

            if (dtoType == null)
                throw new BusinessError("500", $"Не найден тип в сборке: {rootElement}", genericResponse.BusinessProcessState, genericResponse.BusinessProcessCaption);

            object dto;

            try
            {
                dto = XmlSerializationHelpers.LoadData(dtoType, genericResponse.PackedResponse);
            }
            catch (Exception ex)
            {
                throw new BusinessError("500", ex.Message, genericResponse.BusinessProcessState, genericResponse.BusinessProcessCaption);
            }

            if (dto == null)
                return new Entity();

            var availableMappers = Mapper.ConfigurationProvider.GetAllTypeMaps().Where(m => m.SourceType == dtoType).ToList();
            switch (availableMappers.Count)
            {
                case 0:
                    throw new BusinessError("500", $"Не найдена конфигурация маппера для: {dtoType}", genericResponse.BusinessProcessState, genericResponse.BusinessProcessCaption);
                case >= 2:
                    throw new BusinessError("500", $"Найдено больше одной конфигурации маппера для: {dtoType}", genericResponse.BusinessProcessState, genericResponse.BusinessProcessCaption);
            }

            var foundMapper = availableMappers[0];
            var response = (IEntity)Mapper.Map(dto, foundMapper.SourceType, foundMapper.DestinationType);

            response.BusinessProcessState = genericResponse.BusinessProcessState;
            response.BusinessProcessCaption = genericResponse.BusinessProcessCaption;
            response.InfoMessageText = genericResponse.TextMessage;
            return response;
        }

        private async Task<IServiceResponse> CommonCall<TResponse, TRequest>(IEntity entity, TRequest request, string serviceMethod)
            where TResponse : class
        {
            var serviceRequest = request == null 
                ? ServiceRequest.Create(serviceMethod, entity) 
                : ServiceRequest.Create(serviceMethod, request, entity);
            Logger.Debug($"Request method- {serviceRequest.ServiceMethod}");
            Logger.Debug($"Request - {serviceRequest.PackedRequest}");
            var resp = await _service.ExecuteAsync(serviceRequest);
            if (!string.IsNullOrEmpty(resp.BusinessProcessState))
            {
                entity.BusinessProcessState = resp.BusinessProcessState;
                entity.BusinessProcessCaption = resp.BusinessProcessCaption;
                entity.InfoMessageText = resp.TextMessage;
            }

            if (resp.InErrorState)
            {
                Logger.Error($"Unsuccessful call. Reason: {resp.TextMessage}");
                CreateError(resp);
            }
            Logger.Debug($"State - {resp.BusinessProcessState}");
            Logger.Debug($"Method - {resp.ServiceMethod}");
            if(resp.BusinessProcessState != "Сеанс.Настройки")
                Logger.Debug($"Response - {resp.PackedResponse}");
            return typeof(TResponse) == typeof(ResponseStub)
                ? resp
                : ServiceResponse.Create<TResponse>(resp); 
        }
    }
}
