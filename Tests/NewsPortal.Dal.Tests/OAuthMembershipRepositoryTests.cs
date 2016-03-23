using System;
using System.Collections.Generic;
using System.Linq;
using NewsPortal.Dal.Context;
using NewsPortal.Dal.Repository;
using NewsPortal.Dal.UnitOfWork;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NUnit.Framework;

namespace NewsPortal.Dal.Tests
{
    [TestFixture]
    public class OAuthMembershipRepositoryTests
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IOAuthMembershipRepository _oAtuhMembershipRepository;
        private IUserRepository _userRepository;
        private OAuthMembership _testOAuthMembership;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = DalTestsSetup.UnitOfWorkFactory;
            _oAtuhMembershipRepository = new OAuthMembershipRepository(DalTestsSetup.ContextProvider);
            _userRepository = new UserRepository(DalTestsSetup.ContextProvider);
            CreateTestData();
        }

        /// <summary>
        /// Заполнение тестовыми данными
        /// </summary>
        private void CreateTestData()
        {
            _testOAuthMembership = new OAuthMembership()
            {
                OAuthUserId = "TestUserId",
                ProviderName = "TestProvider",
                User = new User {Name = "test1"}
            };
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Create(_testOAuthMembership.User);
                _oAtuhMembershipRepository.Create(_testOAuthMembership);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Получение записи по id пользователя
        /// </summary>
        [Test]
        public void OAuthMembershipRepository_GetByUserIdTest()
        {
            //Arange
            Guid testUserId = _testOAuthMembership.User.Id;
            OAuthMembership resultMembership;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                resultMembership = _oAtuhMembershipRepository.GetWithUser().FirstOrDefault(m => m.User.Id == testUserId);
            }
            //Assert
            Assert.IsNotNull(resultMembership);
            Assert.IsNotNull(resultMembership.User);
            Assert.AreEqual(resultMembership.ProviderName, _testOAuthMembership.ProviderName);
            Assert.AreEqual(resultMembership.OAuthUserId, _testOAuthMembership.OAuthUserId);
            Assert.AreEqual(resultMembership.User.Id, _testOAuthMembership.User.Id);
        }

        /// <summary>
        /// Получение записи по OAuth id пользователя и названию провайдера
        /// </summary>
        [Test]
        public void OAuthMembershipRepository_GetByOAuthIdTest()
        {
            //Arange
            string oAuthUserId = _testOAuthMembership.OAuthUserId;
            string providerName = _testOAuthMembership.ProviderName;
            OAuthMembership resultMembership;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                resultMembership = _oAtuhMembershipRepository
                    .GetWithUser()
                    .FirstOrDefault(m => m.OAuthUserId == oAuthUserId && m.ProviderName == providerName);
            }
            //Assert
            Assert.IsNotNull(resultMembership);
            Assert.IsNotNull(resultMembership.User);
            Assert.AreEqual(resultMembership.ProviderName, _testOAuthMembership.ProviderName);
            Assert.AreEqual(resultMembership.OAuthUserId, _testOAuthMembership.OAuthUserId);
            Assert.AreEqual(resultMembership.User.Id, _testOAuthMembership.User.Id);
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        [Test]
        public void OAuthMembershipRepository_CreateTest()
        {
            //Arange
            User user = new User();
            user.Name = "CreateTest";
            OAuthMembership oAuthMembership = new OAuthMembership();
            oAuthMembership.User = user;
            oAuthMembership.OAuthUserId = "CreateTestUserId";
            oAuthMembership.ProviderName = "CreateTestProviderName";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Create(user);
                _oAtuhMembershipRepository.Create(oAuthMembership);
                unitOfWork.Commit();
            }
            //Assert
            OAuthMembership resultMembership;
            using (_unitOfWorkFactory.Create())
            {
                resultMembership = _oAtuhMembershipRepository.GetWithUser().FirstOrDefault(m => m.Id == oAuthMembership.Id);
            }
            Assert.IsNotNull(resultMembership);
            Assert.IsNotNull(resultMembership.User);
            Assert.AreEqual(resultMembership.ProviderName, oAuthMembership.ProviderName);
            Assert.AreEqual(resultMembership.OAuthUserId, oAuthMembership.OAuthUserId);
            Assert.AreEqual(resultMembership.User.Id, oAuthMembership.User.Id);
        }

        /// <summary>
        /// Изменение записи
        /// </summary>
        [Test]
        public void OAuthMembershipRepository_UpdateTest()
        {
            //Arange
            Guid testMembershipId = _testOAuthMembership.Id;
            OAuthMembership oAuthMembership = new OAuthMembership();
            oAuthMembership.Id = testMembershipId;
            oAuthMembership.OAuthUserId = "UpdateTestUserId";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _oAtuhMembershipRepository.Update(oAuthMembership, "OAuthUserId");
                unitOfWork.Commit();
            }
            //Assert
            OAuthMembership resultMembership;
            using (_unitOfWorkFactory.Create())
            {
                resultMembership = _oAtuhMembershipRepository.GetWithUser().FirstOrDefault(m => m.Id == testMembershipId);
            }
            Assert.IsNotNull(resultMembership);
            Assert.IsNotNull(resultMembership.User);
            Assert.AreEqual(resultMembership.OAuthUserId, oAuthMembership.OAuthUserId);
            Assert.AreEqual(resultMembership.ProviderName, _testOAuthMembership.ProviderName);
            Assert.AreEqual(resultMembership.User.Id, _testOAuthMembership.User.Id);
        }



        [TearDown]
        public void TearDown()
        {
            CleanDataBase();
        }

        /// <summary>
        /// Удаление тестовых данных
        /// </summary>
        private void CleanDataBase()
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _oAtuhMembershipRepository.Delete(_oAtuhMembershipRepository.Get());
                _userRepository.Delete(_userRepository.Get());
                unitOfWork.Commit();
            }
        }
    }
}
