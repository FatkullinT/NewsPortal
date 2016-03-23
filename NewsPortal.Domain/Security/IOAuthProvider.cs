namespace NewsPortal.Domain.Security
{
    public interface IOAuthProvider
    {
        /// <summary>
        /// Имя провайдера
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Перенаправление на страницу аутентификации
        /// </summary>
        /// <param name="callbackUrl"></param>
        void AuthenticationRedirect(string callbackUrl);
    }
}