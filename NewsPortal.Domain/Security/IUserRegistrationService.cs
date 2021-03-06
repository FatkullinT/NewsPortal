using System;

namespace NewsPortal.Domain.Security
{
    public interface IUserRegistrationService
    {
        /// <summary>
        /// Зарегестрировать нового пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="throwIfExist"></param>
        /// <returns></returns>
        Guid Register(string userName, string password, bool throwIfExist = true);
    }
}