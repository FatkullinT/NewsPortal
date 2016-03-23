using System.Configuration;

namespace NewsPortal.Security.ConfigSections
{
	/// <summary>
	/// Класс, описывающий элементы конфигурации
	/// </summary>
	public class OAuthProviderElement : ConfigurationElement 
	{
		[ConfigurationProperty("ProviderName", IsKey = true, IsRequired = true)]
		public string ProviderName
		{
			get { return (string)this["ProviderName"]; }
		}
		[ConfigurationProperty("AppId", IsKey = false, IsRequired = true)]
		public string AppId
		{
			get { return (string)this["AppId"]; }
		}
		[ConfigurationProperty("AppSecret", IsKey = false, IsRequired = true)]
		public string AppSecret
		{
			get { return (string)this["AppSecret"]; }
		}

	}
}
