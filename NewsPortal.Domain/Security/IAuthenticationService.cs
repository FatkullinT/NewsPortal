using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace NewsPortal.Domain.Security
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="persistCookie"></param>
        /// <returns></returns>
        bool Login(string userName, string password, bool persistCookie);

        /// <summary>
        /// Выход из системы
        /// </summary>
        void Logout();
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        string CurrentUserName { get; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        Guid CurrentUserId { get; }

        /// <summary>
        /// Признак аутентификации
        /// </summary>
        bool IsAuthenticated { get; }
    }
}