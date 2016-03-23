using System;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Dal.Repository
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        void AssociateWithUser(Role role, Guid userId);

        void DisassociateWithUser(Role role, Guid userId);
    }
}