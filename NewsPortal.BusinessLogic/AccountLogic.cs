using System;
using System.Linq;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;


namespace NewsPortal.Logic
{
    /// <summary>
    /// Логика работы с настройками учетной записи
    /// </summary>
    public class AccountLogic : IAccountLogic
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthenticationData _authenticationData;


        public AccountLogic(
            IAuthenticationData authenticationData,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAccountRepository accountRepository
)
        {
            _authenticationData = authenticationData;
            _unitOfWorkFactory = unitOfWorkFactory;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Получение настроек текущего пользователя
        /// </summary>
        /// <returns></returns>
        public Account GetAccount()
        {
            if (!_authenticationData.IsAuthenticated)
            {
                return null;
            }
            using (_unitOfWorkFactory.Create())
            {
                return _accountRepository.Get()
                    .FirstOrDefault(userData => userData.UserId == _authenticationData.CurrentUserId) 
                    ?? new Account();
            }
        }

        /// <summary>
        /// Сохранение настроек текущего пользователя
        /// </summary>
        /// <param name="accountRecord"></param>
        public void SaveAccount(Account accountRecord)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                if (accountRecord.Id == Guid.Empty)
                {
                    accountRecord.Id =
                        _accountRepository.Get()
                            .Where(userData => userData.UserId == _authenticationData.CurrentUserId)
                            .Select(userData => userData.Id)
                            .FirstOrDefault();
                }
                if (accountRecord.Id == Guid.Empty)
                {
                    accountRecord.UserId = _authenticationData.CurrentUserId;
                    _accountRepository.Create(accountRecord);
                }
                else
                {
                    _accountRepository.Update(accountRecord, "Email", "SendNews");
                }
                unitOfWork.Commit();
            }
        }
    }
}
