using System;
using System.Collections.Generic;
using NewsPortal.Dal.Context;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Repository
{
    public class RoleRepository : Repository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(ContextProvider contextProvider)
            : base(contextProvider)
        {}

        public void AssociateWithUser(Role role, Guid userId)
        {
            role.Users = role.Users ?? new HashSet<User>();
            User user = new User()
            {
                Id = userId
            };
            EntitySet.Attach(role);
            Context.Users.Attach(user);
            role.Users.Add(user);
        }

        public void DisassociateWithUser(Role role, Guid userId)
        {
            User user = new User()
            {
                Id = userId
            };
            role.Users = role.Users ?? new HashSet<User>() {user};
            EntitySet.Attach(role);
            Context.Users.Attach(user);
            role.Users.Remove(user);
        }
    }
}