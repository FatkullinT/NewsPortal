using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Security.Tests.Fakes
{
    class FakeUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private FakeUnitOfWork _fakeUnitOfWork = new FakeUnitOfWork();
        public List<User> Users = new List<User>();
        public List<Membership> Memberships = new List<Membership>();
        public List<OAuthMembership> OAuthMemberships = new List<OAuthMembership>();
        public List<Role> Roles = new List<Role>(); 

        public IUnitOfWork Create()
        {
            return _fakeUnitOfWork;
        }
    }
}
