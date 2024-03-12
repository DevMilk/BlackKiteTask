using BlackKiteTask.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Handlers
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly ILogger<TokenHandler> _logger;
        public TokenHandler(IConfiguration configuration, IAuthorizationService authorizationService, ILogger<TokenHandler> logger)
        {
            _authorizationService = authorizationService;
            _clientId = configuration["ClientId"];
            _clientSecret = configuration["ClientSecret"];
            _logger = logger;
        }
        public async Task<string> GetAccessToken()
        {
            var resp = await _authorizationService.PostOauthToken(new Requests.Authorization.PostOauthTokenRequest
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                GrantType = "client_credentials"
            });
            return resp.AccessToken;
        }
    }
}
