using System;
using System.Collections.Generic;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;
using NewsPortal.Domain.Security;

namespace NewsPortal.Logic
{
    public class Authentication : IAuthentication
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOAuthService _oAuthService;
        private readonly IAccountLogic _accountLogic;
        private readonly IOAuthInintializer _oAuthInintializer;
        private readonly IRoleProvider _roleProvider;
        private readonly IUserRegistrationService _userRegistrationService;
        
        /// <summary>
        /// Имя роли администратора
        /// </summary>
        internal const string AdministratorRoleName = "Administrator";

        /// <summary>
        /// Логин администратора по умолчанию
        /// </summary>
        private const string DefaultAdminName = "Admin";

        /// <summary>
        /// Пароль администратора по умолчанию
        /// </summary>
        private const string DefaultAdminPassword = "Admin";

        public Authentication(
            IAuthenticationService authenticationService,
            IOAuthService oAuthService,
            IAccountLogic accountLogic,
            IOAuthInintializer oAuthInintializer,
            IRoleProvider roleProvider,
            IUserRegistrationService userRegistrationService)
        {
            _authenticationService = authenticationService;
            _oAuthService = oAuthService;
            _accountLogic = accountLogic;
            _oAuthInintializer = oAuthInintializer;
            _roleProvider = roleProvider;
            _userRegistrationService = userRegistrationService;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Initialize()
        {
            _oAuthInintializer.Initialize();
            SetDefaultUsersAndRoles();
        }

        private void SetDefaultUsersAndRoles()
        {
            Guid userId = _userRegistrationService.Register(DefaultAdminName, DefaultAdminPassword, false);
            _roleProvider.CreateRole(AdministratorRoleName, false);
            _roleProvider.AddUserToRole(userId, AdministratorRoleName);
        }

        /// <summary>
        /// Список имен OAuth-провайдеров
        /// </summary>
        public Dictionary<string, string> OAuthProviderNames
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (IOAuthProvider provider in _oAuthService)
                {
                    result.Add(provider.ProviderName, provider.DisplayName);
                }
                return result;
            }
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        public bool Login(string userName, string password, bool rememberMe)
        {
            return _authenticationService.Login(userName, password, rememberMe);
        }

        /// <summary>
        /// Перенаправление на страницу аутентификации OAuth
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="callBackUrl"></param>
        public void OAuthRedirect(string providerName, string callBackUrl)
        {
            _oAuthService[providerName].AuthenticationRedirect(callBackUrl);
        }

        /// <summary>
        /// Вход в систему по OAuth-аутентификации
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public bool OAuthLogin(out bool newUser)
        {
            OAuthLoginResponse response = _oAuthService.Login();
            if (response.Success && response.IsNewUser)
            {
                _accountLogic.SaveAccount(response.Account);
            }
            newUser = response.IsNewUser;
            return response.Success;
        }

        /// <summary>
        /// Выйти из системы
        /// </summary>
        public void Logout()
        {
            _authenticationService.Logout();
        }
    }
}
