using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;

namespace NewsPortal.Security
{
    /// <summary>
    /// Сервис аутентификации по логину/паролю
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private CookieUserData _cookieUserData;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMembershipRepository _membershipRepository;

        public AuthenticationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IMembershipRepository membershipRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _membershipRepository = membershipRepository;
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid CurrentUserId
        {
            get
            {
                _cookieUserData = _cookieUserData ?? CookieDataProvider.UserData;
                return _cookieUserData != null ? _cookieUserData.UserId : Guid.Empty;
            }
        }

        /// <summary>
        /// Признак аутентификации
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="persistCookie"></param>
        /// <returns></returns>
        public bool Login(string userName, string password, bool persistCookie)
        {
            Membership membership;
            using (_unitOfWorkFactory.Create())
            {
                membership = _membershipRepository.GetWithUser().FirstOrDefault(m => m.User.Name == userName);
            }
            if (membership != null && PasswordEncryptor.Validate(password, membership.Password))
            {
                _cookieUserData = new CookieUserData
                {
                    UserId = membership.User.Id,
                    UserName = membership.User.Name
                };
                CookieDataProvider.Login(membership.User.Name,_cookieUserData, persistCookie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        public void Logout()
        {
            CookieDataProvider.Logout();
            _cookieUserData = null;
        }
    }
}
