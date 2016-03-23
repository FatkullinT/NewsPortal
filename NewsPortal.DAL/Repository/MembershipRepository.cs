using System;
using System.Linq;
using System.Data.Entity;
using NewsPortal.Dal.Context;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Repository
{
    public class MembershipRepository : Repository<Membership, Guid>, IMembershipRepository
    {
        public MembershipRepository(ContextProvider contextProvider) : base(contextProvider)
        {}

        public IQueryable<Membership> GetWithUser()
        {
            return EntitySet.Include(m=>m.User).AsQueryable();
        }
    }
}