using System;
using System.Linq;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Dal.Repository
{
    public interface IOAuthMembershipRepository : IRepository<OAuthMembership, Guid>
    {
        IQueryable<OAuthMembership> GetWithUser();
    }
}