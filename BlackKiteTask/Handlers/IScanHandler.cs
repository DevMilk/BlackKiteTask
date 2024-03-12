using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Handlers
{
    public interface IScanHandler
    {
        public Task Scan(string companyDomain);
    }
}
