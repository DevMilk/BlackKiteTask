using BlackKiteTask.Common.Infrastructure;
using BlackKiteTask.Requests.Company;
using BlackKiteTask.Responses.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Services
{
    public interface ICompanyService
    {
        public Task<PostCompaniesResponse> PostCompany(PostCompaniesRequest request);
        public Task<GetCompaniesResponse> GetCompany(GetCompaniesRequest request);
    }
}
