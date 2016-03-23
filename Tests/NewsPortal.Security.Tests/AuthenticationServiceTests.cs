using System;
using System.IO;
using System.Web;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Security;
using NewsPortal.Security.Tests.Fakes;
using NUnit.Framework;

namespace NewsPortal.Security.Tests
{
    /// <summary>
    /// Тесты сервиса аутентификации
    /// </summary>
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IUserRegistrationService _userRegistrationService;
        private IAuthenticationService _authenticationService;
        private FakeUnitOfWorkFactory _unitOfWorkFactory;


        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = new FakeUnitOfWorkFactory();
            IUserRepository userRepository = new FakeUserRepository(_unitOfWorkFactory);
            IMembershipRepository membershipRepository = new FakeMembershipRepository(_unitOfWorkFactory);
            _userRegistrationService =
                new UserRegistrationService(userRepository, membershipRepository, _unitOfWorkFactory);
            _authenticationService = new AuthenticationService(_unitOfWorkFactory, membershipRepository);
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://localhost", ""),
                new HttpResponse(new StringWriter()));
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
        /// Аутентификация
        /// </summary>
        [Test]
        public void AuthenticationService_LoginSuccessTest()
        {
            //Act
            bool success = _authenticationService.Login(_registeredUserName, _registeredUserPassword, false);
            //Assert
            Assert.IsTrue(success);
            Assert.AreNotEqual(_authenticationService.CurrentUserId, Guid.Empty);
        }

        /// <summary>
        /// Попытка аутентификации с неверным паролем
        /// </summary>
        [Test]
        public void AuthenticationService_LoginFaultTest()
        {
            //Arrange
            string incorrectPassword = "VeryHardPassword";
            //Act
            bool success = _authenticationService.Login(_registeredUserName, incorrectPassword, false);
            //Assert
            Assert.IsFalse(success);
            Assert.AreEqual(_authenticationService.CurrentUserId, Guid.Empty);
        }

    }
}
