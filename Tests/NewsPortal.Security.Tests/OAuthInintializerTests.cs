using System;
using System.IO;
using System.Web;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;
using NewsPortal.Security.Tests.Fakes;
using NUnit.Framework;

namespace NewsPortal.Security.Tests
{
    /// <summary>
    /// Тесты инициалайзера сервиса OAuth
    /// </summary>
    [TestFixture]
    public class OAuthInintializerTests
    {
        private IOAuthInintializer _inintializer;
        private IOAuthService _oAuthService;

        [SetUp]
        public void Setup()
        {
            _inintializer = new OAuthInintializer();
            FakeUnitOfWorkFactory unitOfWorkFactory = new FakeUnitOfWorkFactory();
            IUserRepository userRepository = new FakeUserRepository(unitOfWorkFactory);
            IOAuthMembershipRepository oAuthMembershipRepository = new FakeOAuthMembershipRepository(unitOfWorkFactory);
            _oAuthService = new OAuthService(unitOfWorkFactory, userRepository, oAuthMembershipRepository);
        }

        private string _registeredUserName;
        private string _registeredUserPassword;

        /// <summary>
        /// Инициализация
        /// </summary>
        [Test]
        public void OAuthInintializer_InitializeTest()
        {
            //Arrange
            string providerName = "facebook";
            //Act
            _inintializer.Initialize();
            //Assert
            IOAuthProvider provider = _oAuthService[providerName];
            Assert.IsNotNull(provider);
            Assert.AreEqual(provider.ProviderName, providerName);
        }
    }
}
