using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackKiteTask.Handlers
{
    public interface ITokenHandler
    {
        public Task<string> GetAccessToken();
    }
}
