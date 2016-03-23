using System;
using System.Linq;
using System.Data.Entity;
using NewsPortal.Dal.Context;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Repository
{
    public class OAuthMembershipRepository : Repository<OAuthMembership, Guid>, IOAuthMembershipRepository
    {
        public OAuthMembershipRepository(ContextProvider contextProvider)
            : base(contextProvider)
        {}

        public IQueryable<OAuthMembership> GetWithUser()
        {
            return EntitySet.Include(m=>m.User).AsQueryable();
        }
    }
}