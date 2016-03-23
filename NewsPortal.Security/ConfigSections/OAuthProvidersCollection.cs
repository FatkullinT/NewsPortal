using System.Configuration;

namespace NewsPortal.Security.ConfigSections
{
	/// <summary>
	/// класс описания элементов раздела конфигурации
	/// </summary>
	public class OAuthProvidersCollection : ConfigurationElementCollection 
	{
		/// <summary>
		/// Создание нового элемента
		/// </summary>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new OAuthProviderElement();
		}

	    /// <summary>
		/// Возвращает ключ элемента коллекции
		/// </summary>
		protected override object GetElementKey(ConfigurationElement element)
		{
			OAuthProviderElement setting = (OAuthProviderElement)element;
	        return setting.ProviderName;
		}
		
		/// <summary>
		/// Тип коллекции
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		/// <summary>
		/// Имя элемента в коллекции
		/// </summary>
		protected override string ElementName
		{
			get
			{
				return "OAuthProvider";
			}
		}

		/// <summary>
		/// Индексер для получения типизированных значений
		/// </summary>
		/// <param name="index"></param>
		public OAuthProviderElement this[int index]
		{
			get
			{
				return (OAuthProviderElement)BaseGet(index);
			}
		}
	}
}
