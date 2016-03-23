using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain.Dto
{
    public struct OAuthLoginResponse
    {
        public bool Success;
        public bool IsNewUser;
        public Account Account;
    }
}
