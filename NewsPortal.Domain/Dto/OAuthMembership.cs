using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain.Dto
{
    public class OAuthMembership : Entity<Guid>
    {
        public User User
        {
            get;
            set;
        }

        public string ProviderName
        {
            get;
            set;
        }

        public string OAuthUserId
        {
            get;
            set;
        }
    }
}
