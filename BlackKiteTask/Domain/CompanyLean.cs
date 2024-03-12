using BlackKiteTask.Responses.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlackKiteTask.Domain
{
    public class CompanyLean
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DomainName { get; set; }
        public string Type { get; set; }

        [JsonPropertyName("CyberRating")]
        public CyberRatingModel CyberRating { get; set; }
        public Compliance Compliance { get; set; }
        public FinancialImpact FinancialImpact { get; set; }
        public List<FinancialImpacts> FinancialImpacts { get; set; }
        public string DashboardLink { get; set; }
        public List<Ecosystem> Ecosystems { get; set; }
        public List<License> Licenses { get; set; }
        public Industry Industry { get; set; }
        public string Country { get; set; }
        public List<Tag> Tags { get; set; }
        public string ScanStatus { get; set; }
    }
}
