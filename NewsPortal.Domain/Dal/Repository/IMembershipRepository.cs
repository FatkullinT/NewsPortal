using System;
using System.Linq;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Dal.Repository
{
    public interface IMembershipRepository : IRepository<Membership, Guid>
    {
        IQueryable<Membership> GetWithUser();
    }
}