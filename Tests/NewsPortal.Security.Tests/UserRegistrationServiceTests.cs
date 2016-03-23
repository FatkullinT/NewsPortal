using System.Linq;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;
using NewsPortal.Security.Tests.Fakes;
using NUnit.Framework;

namespace NewsPortal.Security.Tests
{
    /// <summary>
    /// Тесты сервиса рагистрации пользователей
    /// </summary>
    [TestFixture]
    public class UserRegistrationServiceTests
    {
        private IUserRegistrationService _userRegistrationService;
        private FakeUnitOfWorkFactory _unitOfWorkFactory;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = new FakeUnitOfWorkFactory();
            IUserRepository userRepository = new FakeUserRepository(_unitOfWorkFactory);
            IMembershipRepository membershipRepository = new FakeMembershipRepository(_unitOfWorkFactory);
            _userRegistrationService =
                new UserRegistrationService(userRepository, membershipRepository,_unitOfWorkFactory);
            CreateTestData();
        }

        private string _registeredUserName;
        private string _registeredUserPassword;

        private void CreateTestData()
        {
            _registeredUserName = "UserName";
            _registeredUserPassword = "VeryHardPassword123";
            _userRegistrationService.Register(_registeredUserName, _registeredUserPassword);
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [Test]
        public void UserRegistrationService_UserRegistrationTest()
        {
            //Arrange
            string userName = "UserName1";
            string password = "VeryHardPassword123";
            //Act
            _userRegistrationService.Register(userName, password);
            //Assert
            User user = _unitOfWorkFactory.Users.FirstOrDefault(u => string.Equals(u.Name, userName));
            Assert.IsNotNull(user);
            Membership membership = _unitOfWorkFactory.Memberships.FirstOrDefault(m => m.User.Id == user.Id);
            Assert.IsNotNull(membership);
        }

        /// <summary>
        /// Попытка регистрации пользователя с уже существующим логином
        /// </summary>
        [Test]
        public void UserRegistrationService_DoubleUserRegistrationTest()
        {
            Assert.Throws<System.Web.Security.MembershipCreateUserException>(
                () => _userRegistrationService.Register(_registeredUserName, "VeryHardPassword"));
        }
    }
}
