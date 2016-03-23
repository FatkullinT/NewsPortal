using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Logic;
using NewsPortal.Domain.Security;

namespace NewsPortal.Logic
{
    public class AuthenticationData : IAuthenticationData
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleProvider _roleProvider;
        private bool? _isAdministrator;


        public AuthenticationData(
            IAuthenticationService authenticationService,
            IRoleProvider roleProvider)
        {
            _authenticationService = authenticationService;
            _roleProvider = roleProvider;
        }

        /// <summary>
        /// Является ли пользователь администратором
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                return _isAdministrator ??
                           (_isAdministrator =
                               _authenticationService.IsAuthenticated 
                               && _roleProvider.IsUserInRole(_authenticationService.CurrentUserId, Authentication.AdministratorRoleName)).Value;
            }
        }

        /// <summary>
        /// Аутентифицирован ли пользователь
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return _authenticationService.IsAuthenticated;
            }
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return _authenticationService.IsAuthenticated ? _authenticationService.CurrentUserName : null;
            }
        }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid CurrentUserId
        {
            get
            {
                return _authenticationService.CurrentUserId;
            }
        }
    }
}
