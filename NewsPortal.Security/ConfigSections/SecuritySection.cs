using System.Configuration;

namespace NewsPortal.Security.ConfigSections
{
	/// <summary>
	/// класс описания раздела конфигурации
	/// </summary>
	public class SecuritySection : ConfigurationSection
	{

		
		[ConfigurationProperty("OAuthProviders", IsDefaultCollection = true)]
		public OAuthProvidersCollection OAuthProviders
		{
			get
			{
				return (OAuthProvidersCollection)base["OAuthProviders"];
			}
		}	


		/// <summary>
		/// Дефолтовое имя секции в файле настроек
		/// </summary>
		public const string DefaultSectionName = "SecuritySection";

		/// <summary>
		/// Настройка конфигурации
		/// </summary>
		/// <returns></returns>
        public static SecuritySection GetConfiguration()
		{
            return (SecuritySection)ConfigurationManager.GetSection(DefaultSectionName);
		}
	}
}
