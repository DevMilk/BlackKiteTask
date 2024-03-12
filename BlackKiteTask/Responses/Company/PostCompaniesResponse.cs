using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Responses.Company
{
    public class PostCompaniesResponse
    {
        public int CompanyId { get; set; }
        public string MainDomainValue { get; set; }
        public int LicenseId { get; set; }
    }
}
