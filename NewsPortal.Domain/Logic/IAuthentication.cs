using System.Collections.Generic;

namespace NewsPortal.Domain.Logic
{
    public interface IAuthentication
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Initialize();

        /// <summary>
        /// Список имен OAuth-провайдеров
        /// </summary>
        Dictionary<string, string> OAuthProviderNames
        {
            get;
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        bool Login(string userName, string password, bool rememberMe);

        /// <summary>
        /// Перенаправление на страницу аутентификации OAuth
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="callBackUrl"></param>
        void OAuthRedirect(string providerName, string callBackUrl);

        /// <summary>
        /// Вход в систему по OAuth-аутентификации
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        bool OAuthLogin(out bool newUser);

        /// <summary>
        /// Выйти из системы
        /// </summary>
        void Logout();
    }
}