using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Logic;

namespace NewsPortal.Logic.Tests.Fakes
{
    class FakeAuthenticationData : IAuthenticationData
    {
        public bool IsAdministrator
        {
            get;
            set;
        }

        public bool IsAuthenticated
        {
            get;
            set;
        }

        public string CurrentUserName
        {
            get;
            set;
        }

        public Guid CurrentUserId
        {
            get;
            set;
        }
    }
}
