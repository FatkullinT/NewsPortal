using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Security
{
    /// <summary>
    /// Данные о пользователе, хранящиеся в cookie
    /// </summary>
    class CookieUserData
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Токен OAuth
        /// </summary>
        public string OAuthToken
        {
            get;
            set;
        }

        /// <summary>
        /// Имя провайдера OAuth
        /// </summary>
        public string OAuthProviderName
        {
            get;
            set;
        }

        /// <summary>
        /// OAuth - id пользователя
        /// </summary>
        public string OAuthUserId
        {
            get;
            set;
        }
    }
}
