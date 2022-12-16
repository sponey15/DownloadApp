using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Infrastructure
{
    public class AssecoPortalClient : IAssecoPortalClient
    {
        private readonly string _oauthClientUtl;
        private readonly string _oauthUsername;
        private readonly string _oauthPassword;
        private readonly string _assecoPortalUrl;
        private readonly ILogger<AssecoPortalClient> _logger;

        public AssecoPortalClient(IConfiguration config, ILogger<AssecoPortalClient> logger)
        {
            _oauthClientUtl = config["OauthClientUtl"];
            _oauthUsername = config["OauthUsername"];
            _oauthPassword = config["OauthPassword"];
            _assecoPortalUrl = config["AssecoPortalUrl"];
            _logger = logger;
        }

        public async Task<AuthToken> DownloadTokenAsync()
        {
            var expiration = DateTime.Now;

            var response = await GetAuthenticationToken();
            if (response == null) return null;

            var splittedResponse = response.Content.Split(",");
            var splittedAccessToken = splittedResponse[0].Remove(splittedResponse[0].Length - 1, 1)
                                                         .Remove(0, 17);
            var splittedExpiresIn = splittedResponse[2].Remove(0, 13);

            if (!Int32.TryParse(splittedExpiresIn, out int expiresIn)) return null;

            var exp = expiration.AddSeconds(expiresIn);
            var expirationTime = new DateTime(exp.Year, exp.Month, exp.Day, exp.Hour, exp.Minute, exp.Second);

            return new AuthToken
            {
                AccessToken = splittedAccessToken,
                ExpirationTime = expirationTime
            };
        }

        public async Task<string> DownloadFileAsync(string authToken)
        {
            var client = new RestClient(_assecoPortalUrl);
            var request = new RestRequest("", Method.Get);

            request.AddHeader("Authorization", $"Bearer {authToken}");
            request.AddHeader("Cookie", "ROUTEID=.1");

            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK || response.Content == null) 
            {
                _logger.LogError($"There was error downloading file from external server.");
                return null;
            }

            return response.Content;
        }

        private async Task<RestResponse> GetAuthenticationToken()
        {
            var client = new RestClient(_oauthClientUtl);
            var request = new RestRequest("", Method.Post);

            var byteArray = Encoding.ASCII.GetBytes($"{_oauthUsername}:{_oauthPassword}");
            var clientAuthorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            request.AddHeader("Authorization", clientAuthorization.ToString());
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", "USERAPI");

            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
            {
                _logger.LogError($"There was error downloading token from external server.");
                return null;
            }

            return response;
        }
    }
}
