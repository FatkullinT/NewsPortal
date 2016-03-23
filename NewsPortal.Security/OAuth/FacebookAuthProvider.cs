using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;
using Newtonsoft.Json;

namespace NewsPortal.Security
{
    /// <summary>
    /// OAuth-провайдер для facebook
    /// </summary>
    class FacebookAuthProvider : OAuthProviderBase
    {
        private const string Name = "facebook";
        private string _applicationId;
        private string _applicationSecret;
        
        public FacebookAuthProvider(string applicationId, string applicationSecret)
        {
            _applicationId = applicationId;
            _applicationSecret = applicationSecret;
            OAuthService.Add(this);
        }

        public override string AuthorizeUri
        {
            get { return "https://www.facebook.com/dialog/oauth"; }
        }

        public override string TokenUri
        {
            get { return "https://graph.facebook.com/oauth/access_token"; }
        }

        public override string ApplicationId
        {
            get { return _applicationId; }
        }

        public override string ApplicationSecret
        {
            get { return _applicationSecret; }
        }

        public override string ProviderName 
        {
            get
            {
                return Name;
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Facebook";
            }
        }

        public override CookieUserData GetUserData(string token, out Account account)
        {
            Uri uri = new Uri(string.Format("https://graph.facebook.com/me?access_token={0}&fields=name,email", token));
            string response = HttpRequestHelper.SendRequest(uri);
            Dictionary<string, string> facebookUserData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            CookieUserData cookieUserData = new CookieUserData();
            cookieUserData.OAuthProviderName = ProviderName;
            cookieUserData.OAuthToken = token;
            cookieUserData.OAuthUserId = facebookUserData["id"];
            cookieUserData.UserName = facebookUserData["name"];
            account = new Account();
            account.Email = facebookUserData["email"];
            account.SendNews = false;
            return cookieUserData;
        }
    }
}
