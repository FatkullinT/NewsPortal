using System;
using NewsPortal.Dal.Context;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Repository
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(ContextProvider contextProvider)
            : base(contextProvider)
        {}
    }
}