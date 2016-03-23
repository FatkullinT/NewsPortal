using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Сервис аутентификации через OAuth
    /// </summary>
    public class OAuthService : IOAuthService
    {
        private static readonly List<OAuthProviderBase> Providers = new List<OAuthProviderBase>();
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUserRepository _userRepository;
        private readonly IOAuthMembershipRepository _oAuthMembershipRepository;

        public OAuthService(IUnitOfWorkFactory unitOfWorkFactory, IUserRepository userRepository, IOAuthMembershipRepository oAuthMembershipRepository)
        {
            _oAuthMembershipRepository = oAuthMembershipRepository;
            _userRepository = userRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Добавление OAuth-провайдера
        /// </summary>
        /// <param name="provider"></param>
        internal static void Add(OAuthProviderBase provider)
        {
            Providers.Add(provider);
        }

        /// <summary>
        /// Получение OAuth провайдера по имени
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public IOAuthProvider this[string providerName]
        {
            get
            {
                return this.Single(provider => string.Equals(provider.ProviderName, providerName));
            }
        }

        /// <summary>
        /// Аутентификация по полученному токену
        /// </summary>
        /// <returns></returns>
        public OAuthLoginResponse Login()
        {
            OAuthLoginResponse loginResponse = new OAuthLoginResponse();
            string providerName = HttpContext.Current.Request.Params["providerName"];
            OAuthProviderBase oAuthProvider = Providers.FirstOrDefault(provider => string.Equals(provider.ProviderName, providerName));
            if (oAuthProvider == null)
            {
                loginResponse.Success = false;
            }
            else
            {
                NameValueCollection response = oAuthProvider.SendTokenRequest();
                string token = response["access_token"];
                if (string.IsNullOrEmpty(token))
                {
                    loginResponse.Success = false;
                }
                else
                {
                    DateTime tokenExpires = GetTokenExpires(response);
                    CookieUserData cookieUserData = oAuthProvider.GetUserData(token, out loginResponse.Account);
                    User user = FindOrCreateUser(cookieUserData, out loginResponse.IsNewUser);
                    cookieUserData.UserId = user.Id;
                    CookieDataProvider.Login(user.Name, cookieUserData, tokenExpires);
                    loginResponse.Success = true;
                }
                
            }
            
            return loginResponse;
        }

        /// <summary>
        /// Поиск существующего и создание нового пользователя
        /// </summary>
        /// <param name="cookieUserData"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
        private User FindOrCreateUser(CookieUserData cookieUserData, out bool newUser)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                OAuthMembership oAuthMembership =
                    _oAuthMembershipRepository.GetWithUser()
                        .FirstOrDefault(
                            m =>
                                m.OAuthUserId == cookieUserData.OAuthUserId &&
                                m.ProviderName == cookieUserData.OAuthProviderName);
                if (oAuthMembership != null)
                {
                    newUser = false;
                    return oAuthMembership.User;
                }
                User user = new User();
                user.Name = cookieUserData.UserName;
                OAuthMembership membership = new OAuthMembership();
                membership.User = user;
                membership.ProviderName = cookieUserData.OAuthProviderName;
                membership.OAuthUserId = cookieUserData.OAuthUserId;
                _userRepository.Create(user);
                _oAuthMembershipRepository.Create(membership);
                unitOfWork.Commit();
                newUser = true;
                return user;
            }
        }

        /// <summary>
        /// Получение даты истечения срока действия токена
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static DateTime GetTokenExpires(NameValueCollection response)
        {
            int expireSeconds;
            DateTime tokenExpires;
            if (int.TryParse(response["expires"], out expireSeconds))
            {
                tokenExpires = DateTime.Now.AddSeconds(expireSeconds);
            }
            else
            {
                tokenExpires = DateTime.Now.AddDays(30);
            }
            return tokenExpires;
        }

        public IEnumerator<IOAuthProvider> GetEnumerator()
        {
            return Providers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
