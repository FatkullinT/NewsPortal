using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Security.Tests.Fakes
{
    class FakeRoleRepository : FakeRepositoryBase<Role>, IRoleRepository
    {
        public FakeRoleRepository(FakeUnitOfWorkFactory fakeUnitOfWorkFactory) : base(fakeUnitOfWorkFactory)
        {
        }

        public override void Create(Role role)
        {
            role.Users = role.Users ?? new List<User>();
            base.Create(role);
        }

        protected override List<Role> EntityCollection
        {
            get
            {
                return FakeUnitOfWorkFactory.Roles;
            }
        }

        public void AssociateWithUser(Role role, Guid userId)
        {
            Role roleOnAssociate = EntityCollection.Find(r => r.Id == role.Id);
            User userOnAssociate = FakeUnitOfWorkFactory.Users.Find(u => u.Id == userId);
            roleOnAssociate.Users = roleOnAssociate.Users ?? new List<User>();
            userOnAssociate.Roles = userOnAssociate.Roles ?? new List<Role>();
            roleOnAssociate.Users.Add(userOnAssociate);
            userOnAssociate.Roles.Add(roleOnAssociate);
        }

        public void DisassociateWithUser(Role role, Guid userId)
        {
            Role roleOnDisassociate = EntityCollection.Find(r => r.Id == role.Id);
            User userOnDisassociate = FakeUnitOfWorkFactory.Users.Find(u => u.Id == userId);
            roleOnDisassociate.Users.Remove(userOnDisassociate);
            userOnDisassociate.Roles.Remove(roleOnDisassociate);
        }
    }
}
