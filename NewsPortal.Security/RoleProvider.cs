using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;

namespace NewsPortal.Security
{
    public class RoleProvider : IRoleProvider
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public RoleProvider(IRoleRepository roleRepository, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _roleRepository = roleRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Имеет ли пользователь роль
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsUserInRole(Guid userId, string roleName)
        {
            using (_unitOfWorkFactory.Create())
            {
                return IsUserInRoleRequest(userId, roleName);
            }
        }
        
        private bool IsUserInRoleRequest(Guid userId, string roleName)
        {
            return _roleRepository.Get()
                .Where(r => r.Name == roleName)
                .SelectMany(r => r.Users)
                .Any(u => u.Id == userId);
        }

        /// <summary>
        /// Вернусть все роли для данного пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string[] GetRolesForUser(Guid userId)
        {
            using (_unitOfWorkFactory.Create())
            {
                return _roleRepository.Get().Where(r => r.Users.Any(u => u.Id == userId)).Select(r=>r.Name).ToArray();
            }
        }

        /// <summary>
        /// Создать роль безопасности
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="throwIfExist"></param>
        public void CreateRole(string roleName, bool throwIfExist)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                if (_roleRepository.Get().Any(r => r.Name == roleName))
                {
                    if (throwIfExist)
                    {
                        throw new Exception(string.Format("Роль с именем {0} уже существует", roleName));
                    }
                    return;
                }
                Role role = new Role
                {
                    Name = roleName
                };
                _roleRepository.Create(role);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Добавить роль пользователю
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        public void AddUserToRole(Guid userId, string roleName)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                Role role = _roleRepository.Get().FirstOrDefault(r => r.Name == roleName);
                if (role != null && !IsUserInRoleRequest(userId, roleName))
                {
                   _roleRepository.AssociateWithUser(role, userId);
                    unitOfWork.Commit();
                }
            }
        }

        /// <summary>
        /// Убрать роль у пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        public void RemoveUserFromRole(Guid userId, string roleName)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                Role role = _roleRepository.Get().FirstOrDefault(r => r.Name == roleName);
                if (role != null && IsUserInRoleRequest(userId, roleName))
                {
                    _roleRepository.DisassociateWithUser(role, userId);
                    unitOfWork.Commit();
                }
            }
        }
    }
}
