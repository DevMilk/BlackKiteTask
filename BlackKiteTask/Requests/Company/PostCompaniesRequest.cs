using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlackKiteTask.Domain.Enums;

namespace BlackKiteTask.Requests.Company
{
    public class PostCompaniesRequest
    {
        public string MainDomainValue { get; set; }
        public string CompanyName { get; set; }
        public int EcosystemId { get; set; }
        public string LicenseType { get; set; }
        public bool IsSubsidiary { get; set; }
        public bool IsCloudProvider { get; set; }

    }
}
