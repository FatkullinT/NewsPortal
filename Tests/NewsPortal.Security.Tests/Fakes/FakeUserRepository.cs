using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Security.Tests.Fakes
{
    class FakeUserRepository : FakeRepositoryBase<User>, IUserRepository
    {
        public FakeUserRepository(FakeUnitOfWorkFactory fakeUnitOfWorkFactory) : base(fakeUnitOfWorkFactory)
        {}

        public override void Create(User user)
        {
            user.Roles = user.Roles ?? new List<Role>();
            base.Create(user);
        }

        protected override List<User> EntityCollection
        {
            get
            {
                return FakeUnitOfWorkFactory.Users;
            }
        }
    }
}
