using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Domain
{
    public class FinancialImpact
    {
        public float? Rating { get; set; }
        public float? RatingMin { get; set; }
        public float? RatingMax { get; set; }
        public float? LossMagnitude { get; set; }
        public float? LossEventFrequency { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
