using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;

namespace NewsPortal.Security
{
    /// <summary>
    /// Базовый OAuth-провайдер
    /// </summary>
    abstract class OAuthProviderBase : IOAuthProvider
    {
        public abstract string AuthorizeUri { get; }
        public abstract string TokenUri { get; }
        public abstract string ApplicationId { get; }
        public abstract string ApplicationSecret { get; }
        public abstract string ProviderName { get; }
        public abstract string DisplayName { get; }

        /// <summary>
        /// Перенаправление на страницу аутентификации
        /// </summary>
        /// <param name="callbackUrl"></param>
        public void AuthenticationRedirect(string callbackUrl)
        {
            string url = string.Format("{0}?client_id={1}&scope=email&redirect_uri={2}?providerName={3}", 
                AuthorizeUri,
                ApplicationId, 
                HttpRequestHelper.ConvertUriToAbsolute(callbackUrl), 
                ProviderName);
            HttpRequestHelper.Redirect(url);
        }

        /// <summary>
        /// Отправка запроса на получение токена
        /// </summary>
        /// <returns></returns>
        public NameValueCollection SendTokenRequest()
        {
            Uri uri = TokenRequestUri();
            string response = HttpRequestHelper.SendRequest(uri);
            return HttpUtility.ParseQueryString(response);
        }

        /// <summary>
        /// Url для получения токена
        /// </summary>
        /// <returns></returns>
        private Uri TokenRequestUri()
        {
            var url = string.Format("{0}?client_id={1}&client_secret={2}&code={3}&redirect_uri={4}?providerName={5}",
                TokenUri, 
                ApplicationId, 
                ApplicationSecret, 
                HttpRequestHelper.GetRequestParameter("code"),
                HttpRequestHelper.ConvertUriToAbsolute(HttpRequestHelper.RequestPath), 
                ProviderName);
            return new Uri(url);
        }

        /// <summary>
        /// Получение данных о пользователе из соц-сетерй
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="account">Дополнительные данные</param>
        /// <returns>Основные данные</returns>
        public abstract CookieUserData GetUserData(string token, out Account account);
    }
}
