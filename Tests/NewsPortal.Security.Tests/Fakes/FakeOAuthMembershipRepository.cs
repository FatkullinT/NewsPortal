using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Security.Tests.Fakes
{
    class FakeOAuthMembershipRepository : FakeRepositoryBase<OAuthMembership>, IOAuthMembershipRepository
    {
        public FakeOAuthMembershipRepository(FakeUnitOfWorkFactory fakeUnitOfWorkFactory) : base(fakeUnitOfWorkFactory)
        {
        }

        public override IQueryable<OAuthMembership> Get()
        {
            return
                EntityCollection.Select(
                    m => new OAuthMembership {Id = m.Id, ProviderName = m.ProviderName, OAuthUserId = m.OAuthUserId})
                    .AsQueryable();
        }

        public override OAuthMembership GetById(Guid id)
        {
            return Get().FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<OAuthMembership> GetWithUser()
        {
            return base.Get();
        }

        protected override List<OAuthMembership> EntityCollection
        {
            get
            {
                return FakeUnitOfWorkFactory.OAuthMemberships;
            }
        }
    }
}
