using System;
using System.Collections.Generic;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Security
{
    public interface IOAuthService : IEnumerable<IOAuthProvider>
    {
        IOAuthProvider this[string providerName] { get; }

        /// <summary>
        /// Аутентифицироваться, используя токен OAuth
        /// </summary>
        /// <returns></returns>
        OAuthLoginResponse Login();
    }
}