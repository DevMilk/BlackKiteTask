using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Domain
{
    public class Enums
    {
        public enum ScanStatus
        {
            [Description("Preliminary Scan Queued")]
            PreliminaryScanQueued,
            [Description("Preliminary Scan Running")]
            PreliminaryScanRunning,
            [Description("Preliminary Scan Failed")]
            PreliminaryScanFailed,
            [Description("Preliminary Results Ready")]
            PreliminaryResultsReady,

            [Description("Extended Scan Queued")]
            ExtendedScanQueued,
            [Description("Extended Scan Running")]
            ExtendedScanRunning,
            [Description("Extended Scan Failed")]
            ExtendedScanFailed,

            [Description("Extended Results Ready")]
            ExtendedResultsReady,

            [Description("Extended Rescan Queued")]
            ExtendedRescanQueued,
            [Description("Extended Rescan Running")]
            ExtendedRescanRunning,
            [Description("Extended Rescan Failed")]
            ExtendedRescanFailed,
            [Description("Extended Rescan Results Ready")]
            ExtendedRescanResultsReady,

            [Description("Unknown Scan Status")]
            UnknownScanStatus
        }
    }
}
