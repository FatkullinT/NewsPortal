using System;

namespace NewsPortal.Domain.Logic
{
    public interface IAuthenticationData
    {

        /// <summary>
        /// Является ли пользователь администратором
        /// </summary>
        bool IsAdministrator
        {
            get;
        }

        /// <summary>
        /// Аутентифицирован ли пользователь
        /// </summary>
        bool IsAuthenticated
        {
            get;
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        string CurrentUserName
        {
            get;
        }

        /// <summary>
        /// Id пользователя
        /// </summary>
        Guid CurrentUserId
        {
            get;
        }
    }
}