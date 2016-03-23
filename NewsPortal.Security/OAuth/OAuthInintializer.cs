using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Security;
using NewsPortal.Security.ConfigSections;

namespace NewsPortal.Security
{
    /// <summary>
    /// Инициализатор OAuth-провайдеров
    /// </summary>
    public class OAuthInintializer : IOAuthInintializer
    {
        /// <summary>
        /// Список имен OAuth-провайдеров и их типов
        /// </summary>
        private Dictionary<string, Type> _oAuthProviders = new Dictionary<string, Type>()
        {
            {"facebook", typeof (FacebookAuthProvider)}
        };

        /// <summary>
        /// Инициализация OAuth-провайдеров
        /// </summary>
        public void Initialize()
        {
            OAuthProvidersCollection oAuthProviders = SecuritySection.GetConfiguration().OAuthProviders;
            foreach (OAuthProviderElement oAuthProviderElement in oAuthProviders)
            {
                string oAuthProviderName = oAuthProviderElement.ProviderName;
                if (_oAuthProviders.ContainsKey(oAuthProviderName))
                {
                    Activator.CreateInstance(_oAuthProviders[oAuthProviderName],
                        oAuthProviderElement.AppId,
                        oAuthProviderElement.AppSecret);
                }
                else
                {
                    throw new Exception(string.Format("Не найден OAuth-провайдер с именем {0}", oAuthProviderName));
                }
            }
        }
    }
}
