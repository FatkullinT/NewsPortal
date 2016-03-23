using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace NewsPortal.Security
{
    static class CookieDataProvider
    {
        private const string AuthCookieName = ".ASPXAUTH";

        public static void Login(string userName, CookieUserData userData, bool rememberMe)
        {
            DateTime expiresDate = DateTime.Now.AddHours(2);
            if (rememberMe)
            {
                expiresDate = expiresDate.AddDays(10);
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                2,
                userName,
                DateTime.Now,
                expiresDate,
                rememberMe,
                Serialize(userData));
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            SetCookie(AuthCookieName, encryptedTicket, expiresDate);
        }

        public static void Login(string userName,  CookieUserData userData, DateTime expiresDate)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                2,
                userName,
                DateTime.Now,
                expiresDate,
                true,
                Serialize(userData));
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            SetCookie(AuthCookieName, encryptedTicket, expiresDate);
        }

        public static void Logout()
        {
            SetCookie(AuthCookieName, null, DateTime.Now.AddYears(-1));
        }

        public static CookieUserData UserData
        {
            get
            {
                string cookie = HttpContext.Current.Request.Cookies[AuthCookieName] != null
                    ? HttpContext.Current.Request.Cookies[AuthCookieName].Value
                    : null;
                if (cookie != null && !string.IsNullOrEmpty(cookie))
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie);
                    if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                    {
                        CookieUserData userData = Deserialize(ticket.UserData);
                        return userData;
                    }
                }
                return null;
            }
        }

        private static void SetCookie(string cookieName, string cookieObject, DateTime dateStoreTo)
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Path = "/";
            }
            cookie.Value = cookieObject;
            cookie.Expires = dateStoreTo;
            HttpContext.Current.Response.SetCookie(cookie);
        }

        private static string Serialize(CookieUserData userData)
        {
            return JsonConvert.SerializeObject(userData);
        }

        private static CookieUserData Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<CookieUserData>(jsonString);
        }
    }
}
