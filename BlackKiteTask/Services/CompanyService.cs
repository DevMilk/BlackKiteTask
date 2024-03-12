using Azure.Core;
using BlackKiteTask.Common.Infrastructure;
using BlackKiteTask.Requests.Company;
using BlackKiteTask.Responses.Company;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlackKiteTask.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly HttpClient _client;
        public CompanyService(HttpClient client) {
            _client = client;
        }
        

        public async Task<PostCompaniesResponse> PostCompany(PostCompaniesRequest request)
        {
            var httpResponse = await _client.PostAsyncSerialized("companies", request);
            return await httpResponse.DeserializeAsync<PostCompaniesResponse>();
        }

        public async Task<GetCompaniesResponse> GetCompany(GetCompaniesRequest request)
        {
            var httpResponse = await _client.GetAsync($"companies/{request.Id}");
            return await httpResponse.DeserializeAsync<GetCompaniesResponse>();
        }
    }
}
