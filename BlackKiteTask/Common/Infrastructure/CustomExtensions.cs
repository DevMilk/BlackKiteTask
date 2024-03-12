using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlackKiteTask.Common.Infrastructure
{
    public static class CustomExtensions
    {
        private static readonly JsonSerializerOptions _defaultSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response)
        {

            string stringContent = "";
            try
            {
                stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var deserializedResponse = JsonSerializer.Deserialize<T>(stringContent);
                return deserializedResponse;
            } catch(HttpRequestException httpEx)
            {
                var errorModel = new ErrorModel();
                try
                {
                    var deserializedErrorResponse = JsonSerializer.Deserialize<ErrorModel>(stringContent);
                    errorModel.Message = deserializedErrorResponse.Message;
                } catch(Exception _)
                {
                    //If httpException doesnt have message:
                    errorModel.Message = "Unparseable http exception message: " + httpEx.Message;
                }
                throw new HttpRequestException(errorModel.Message, httpEx, response.StatusCode);

            }

        }

        public static async Task<HttpResponseMessage> PostAsyncSerialized<T>(this HttpClient client, string uri, T request)
        {
            var payload = new StringContent(JsonSerializer.Serialize(request, _defaultSerializerOptions), Encoding.UTF8, "application/json");
            return await client.PostAsync(uri, payload);
        }
        public static void LogErrorFormated(this ILogger logger, Exception exc)
        {
            switch (exc)
            {
                case HttpRequestException:
                    var ex = exc as HttpRequestException;
                    logger.LogError("HttpRequest error: [{StatusCode} : {Status}] : [{Message}]", 
                        (int)ex.StatusCode, ex.StatusCode, ex.Message);
                    break;
                default:
                    logger.LogError("Unknown error: Error Message:[{Message}]", 
                        exc.Message);
                    break;
            }
        }

    }
}
