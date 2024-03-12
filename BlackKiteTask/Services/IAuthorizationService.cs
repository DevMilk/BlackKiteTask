using BlackKiteTask.Common.Infrastructure;
using BlackKiteTask.Requests.Authorization;
using BlackKiteTask.Responses.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Services
{
    public interface IAuthorizationService
    {
        public Task<PostOauthTokenResponse> PostOauthToken(PostOauthTokenRequest request);
    }
}
