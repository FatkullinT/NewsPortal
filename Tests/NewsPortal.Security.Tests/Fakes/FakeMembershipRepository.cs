using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Security.Tests.Fakes
{
    class FakeMembershipRepository : FakeRepositoryBase<Membership>, IMembershipRepository
    {
        public FakeMembershipRepository(FakeUnitOfWorkFactory fakeUnitOfWorkFactory) : base(fakeUnitOfWorkFactory)
        {}

        public override IQueryable<Membership> Get()
        {
            return EntityCollection.Select(m => new Membership {Id = m.Id, Password = m.Password}).AsQueryable();
        }

        public override Membership GetById(Guid id)
        {
            return Get().FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<Membership> GetWithUser()
        {
            return base.Get();
        }

        protected override List<Membership> EntityCollection
        {
            get
            {
                return FakeUnitOfWorkFactory.Memberships;
            }
        }
    }
}
