using System;

namespace NewsPortal.Domain.Security
{
    public interface IRoleProvider
    {
        /// <summary>
        /// Имеет ли пользователь роль
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        bool IsUserInRole(Guid userId, string roleName);

        /// <summary>
        /// Получить все роли для пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string[] GetRolesForUser(Guid userId);

        /// <summary>
        /// Создать роль
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="throwIfExist"></param>
        void CreateRole(string roleName, bool throwIfExist);

        /// <summary>
        /// Добавить роль пользователю
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        void AddUserToRole(Guid userId, string roleName);

        /// <summary>
        /// Убрать роль у пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        void RemoveUserFromRole(Guid userId, string roleName);
    }
}