using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;

namespace NewsPortal.Security
{
    /// <summary>
    /// Сервис регистрации пользователей
    /// </summary>
    public class UserRegistrationService : IUserRegistrationService
    {
        private IUserRepository _userRepository;
        private IMembershipRepository _membershipRepository;
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public UserRegistrationService(IUserRepository userRepository,
            IMembershipRepository membershipRepository,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _userRepository = userRepository;
            _membershipRepository = membershipRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Зарегестрировать нового пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="throwIfExist">Выдать исключение, если пользователь с таким именем существует. Ничего не делать, если false</param>
        /// <returns></returns>
        public Guid Register(string userName, string password, bool throwIfExist = true)
        {
            Membership membership;
            using (_unitOfWorkFactory.Create())
            {
                membership = _membershipRepository.GetWithUser().FirstOrDefault(m => m.User.Name == userName);
                if (membership != null)
                {
                    if (throwIfExist)
                    {
                        throw new System.Web.Security.MembershipCreateUserException("Пользователь с таким именем уже существует.");
                    }
                    return membership.User.Id;
                }
            }
            User user = new User()
            {
                Name = userName
            };
            membership = new Membership()
            {
                User = user,
                Password = PasswordEncryptor.Encrypt(password)
            };
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Create(user);
                _membershipRepository.Create(membership);
                unitOfWork.Commit();
            }
            return user.Id;
        }
    }
}
