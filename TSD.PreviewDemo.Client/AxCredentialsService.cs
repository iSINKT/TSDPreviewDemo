#nullable enable
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.DataLayer.Interfaces.Services;

namespace TSD.PreviewDemo.Client
{
    public class AxCredentialsService(ILogger logger, IRestClient restClient) :
        INetworkCredentials,
        IWarmAbleService
    {
        private readonly ILogger _logger = logger.ThrowIfNull(nameof(logger));
        private readonly IRestClient _restClient = restClient.ThrowIfNull(nameof(restClient));

        public async Task<NetworkCredential> GetCredential()
        {
            var authRequest = new RestRequest();
            var response = await _restClient.ExecuteAsync(authRequest);

            if (!response.IsSuccessful)
            {
                _logger.Error($"Ошибка получения учетных данных: {response.ErrorMessage}");
                throw new AuthenticationException("Ошибка получения учетных данных.", response.ErrorException);
            }

            var authHeader = response.Headers.FirstOrDefault(p => p.Name == "TSD_DAX_Credentials");

            if (authHeader == null)
            {
                _logger.Error("Ошибка получения учетных данных из заголовка.");
                throw new AuthenticationException("Ошибка получения учетных данных из заголовка.");
            }

            var cr = authHeader.Value?.ToString().Split(';');

            if (cr is { Length: 3 })
            {
                return new NetworkCredential(cr[0], cr[1], cr[2]);
            }

            _logger.Error($"Ошибка получения учетных данных из заголовка: {authHeader.Value}");
            throw new AuthenticationException($"Ошибка получения учетных данных из заголовка: {authHeader.Value}");
        }

        public void WarmUp()
        {
            throw new NotImplementedException();
        }
    }
}
