using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Logic
{
    public interface IAccountLogic
    {
        /// <summary>
        /// ѕолучение настроек текущего пользовател€
        /// </summary>
        /// <returns></returns>
        Account GetAccount();

        /// <summary>
        /// —охранение настроек текущего пользовател€
        /// </summary>
        /// <param name="accountRecord"></param>
        void SaveAccount(Account accountRecord);
    }
}