using BlackKiteTask.Common.Infrastructure;
using BlackKiteTask.Requests.Authorization;
using BlackKiteTask.Responses.Authorization;
using BlackKiteTask.Responses.Company;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly HttpClient _client;
        public AuthorizationService(HttpClient client)
        {
            _client = client;
        }

        public async Task<PostOauthTokenResponse> PostOauthToken(PostOauthTokenRequest request)
        {
            var httpResponse = await _client.PostAsyncSerialized("token", request);
            return await httpResponse.DeserializeAsync<PostOauthTokenResponse>();
        }
    }
}
