using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlackKiteTask.Requests.Authorization
{
    public class PostOauthTokenRequest
    {

        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }
    }
}
