using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Domain
{
    public class Compliance
    {
        public int? Rating { get; set; }
        public float? Confidence { get; set; }
        public float? Completeness { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
