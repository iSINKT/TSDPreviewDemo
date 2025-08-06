using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using TSD.PreviewDemo.Client.Exceptions;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;
using TSD.PreviewDemo.DataLayer.Services;

namespace TSD.PreviewDemo.Client.Wrappers
{
    public abstract class ServiceWrapper(
        ILogger logger,
        IEndpointAddressResolver endpointAddressResolver,
        ServiceType serviceType)
    {
        private readonly ILogger _logger = logger.ThrowIfNull(nameof(logger));
        protected readonly IEndpointAddressResolver EndpointAddressResolver = endpointAddressResolver.ThrowIfNull(nameof(endpointAddressResolver));

        protected async Task<TResponse> CallService<TResponse>(Func<TResponse> serviceAction) where TResponse : class
        {
            TResponse response = null;
            await Task.Factory.StartNew(() => CallService(null, serviceAction, out response));
            return response;
        }

        protected TResponse CallServiceSync<TResponse>(Func<TResponse> serviceAction) where TResponse : class
        {
            CallService(null, serviceAction, out var response);
            return response;
        }

        private void CallService<TResponse>(Action serviceAction, Func<TResponse> serviceActionWithResp, out TResponse result) where TResponse : class
        {
            result = null;
            try
            {
                if (serviceActionWithResp != null)
                {
                    result = serviceActionWithResp();
                    return;
                }

                serviceAction?.Invoke();
            }
            catch (WebException exception)
            {
                string errCode;
                switch (exception.Status)
                {
                    case WebExceptionStatus.ProtocolError when exception.Response is HttpWebResponse response:
                        {
                            errCode = response.StatusCode switch
                            {
                                HttpStatusCode.Unauthorized => "401",
                                HttpStatusCode.RequestTimeout => "408",
                                HttpStatusCode.ServiceUnavailable => "503",
                                HttpStatusCode.InternalServerError => "500",
                                HttpStatusCode.NotFound => "404",
                                _ => "2017"
                            };

                            break;
                        }
                    case WebExceptionStatus.ProtocolError:
                        errCode = "2018";
                        break;
                    case WebExceptionStatus.Timeout:
                        errCode = "4081";
                        break;
                    case WebExceptionStatus.NameResolutionFailure:
                    case WebExceptionStatus.ConnectFailure:
                        errCode = "2022";
                        break;
                    default:
                        errCode = "2019";
                        break;
                }

                var errMsg =
                    $"WebException: Url: {EndpointAddressResolver.Resolve().AbsoluteUri}, ErrorCode: {errCode} Exception: {exception.Message}";
                _logger.Trace("ServiceWrapper->->WebException: URL:{0}, ErrorCode: {1}, Exception: {2}",
                    EndpointAddressResolver.Resolve().AbsoluteUri, errCode, exception.Message);
                throw new ServiceException(EndpointAddressResolver.Resolve(), serviceType, errMsg, errCode, exception);
            }
            catch (CommunicationException exception)
            {
                _logger.Trace("ServiceWrapper->CommunicationException: URL:{0}, Exception: {1}", EndpointAddressResolver, exception.Message);

                var errMsg = $"CommunicationException: URL:{EndpointAddressResolver.Resolve()}, Exception: {exception.Message}";
                var errCode = "500";
                throw new ServiceException(EndpointAddressResolver.Resolve(), ServiceType.Unknown, errMsg, errCode, exception);
            }
            catch (TimeoutException exception)
            {
                _logger.Trace("ServiceWrapper->->TimeoutException: URL:{0}, Exception: {1}", EndpointAddressResolver, exception.Message);

                var errMsg = $"TimeoutException: URL:{EndpointAddressResolver}, Exception: {exception.Message}";
                var errCode = "4081";
                throw new ServiceException(EndpointAddressResolver.Resolve(), serviceType, errMsg, errCode, exception);
            }
            catch (Exception exception)
            {
                const string errCode = "500";
                var errMsg =
                    $"Exception: Url: {EndpointAddressResolver.Resolve().AbsoluteUri}, ErrorCode: {errCode} Exception: {exception.Message}";
                _logger.Trace("ServiceWrapper->->Exception: URL:{0}, ErrorCode: {1}, Exception: {2}", EndpointAddressResolver.Resolve().AbsoluteUri,
                    errCode, exception.Message);
                throw new ServiceException(EndpointAddressResolver.Resolve(), serviceType, errMsg, errCode, exception);
            }
        }
    }
}
