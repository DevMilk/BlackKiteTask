using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Domain
{
    public class CyberRatingModel
    {
        public string GradeLetter { get; set; }
        public float? CyberRating { get; set; }
        public float? BreachIndex { get; set; }
        public float? RansomwareIndex { get; set; }
        public DateTime? CyberRatingLastUpdatedAt { get; set; }
        public DateTime? RansomwareIndexLastUpdatedAt { get; set; }
        public DateTime? BreachIndexLastUpdatedAt { get; set; }
    }
}
