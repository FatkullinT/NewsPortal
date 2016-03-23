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
    public class MembershipRepositoryTests
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IMembershipRepository _membershipRepository;
        private IUserRepository _userRepository;
        private Membership _testMembership;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = DalTestsSetup.UnitOfWorkFactory;
            _membershipRepository = new MembershipRepository(DalTestsSetup.ContextProvider);
            _userRepository = new UserRepository(DalTestsSetup.ContextProvider);
            CreateTestData();
        }

        /// <summary>
        /// Заполнение тестовыми данными
        /// </summary>
        private void CreateTestData()
        {
            _testMembership = new Membership() {Password = "Password1", User = new User {Name = "test1"}};
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Create(_testMembership.User);
                _membershipRepository.Create(_testMembership);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Получение записи по id пользователя
        /// </summary>
        [Test]
        public void MembershipRepository_GetByUserIdTest()
        {
            //Arange
            Guid testUserId = _testMembership.User.Id;
            Membership resultMembership;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                resultMembership = _membershipRepository.GetWithUser().FirstOrDefault(m => m.User.Id == testUserId);
            }
            //Assert
            Assert.IsNotNull(resultMembership);
            Assert.IsNotNull(resultMembership.User);
            Assert.AreEqual(resultMembership.Password, _testMembership.Password);
            Assert.AreEqual(resultMembership.User.Id, _testMembership.User.Id);
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        [Test]
        public void MembershipRepository_CreateTest()
        {
            //Arange
            User user = new User();
            user.Name = "CreateTest";
            Membership membership = new Membership();
            membership.User = user;
            membership.Password = "TestPassword";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Create(user);
                _membershipRepository.Create(membership);
                unitOfWork.Commit();
            }
            //Assert
            Membership resultMembership;
            using (_unitOfWorkFactory.Create())
            {
                resultMembership = _membershipRepository.GetWithUser().FirstOrDefault(m => m.Id == membership.Id);
            }
            Assert.IsNotNull(resultMembership);
            Assert.IsNotNull(resultMembership.User);
            Assert.AreEqual(resultMembership.Password, membership.Password);
            Assert.AreEqual(resultMembership.User.Id, membership.User.Id);
        }

        /// <summary>
        /// Изменение записи
        /// </summary>
        [Test]
        public void MembershipRepository_UpdateTest()
        {
            //Arange
            Guid testMembershipId = _testMembership.Id;
            Membership membership = new Membership();
            membership.Id = testMembershipId;
            membership.Password = "UpdateTest";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _membershipRepository.Update(membership, "Password");
                unitOfWork.Commit();
            }
            //Assert
            Membership resultMembership;
            using (_unitOfWorkFactory.Create())
            {
                resultMembership = _membershipRepository.GetWithUser().FirstOrDefault(m => m.Id == testMembershipId);
            }
            Assert.IsNotNull(resultMembership);
            Assert.IsNotNull(resultMembership.User);
            Assert.AreEqual(resultMembership.Password, membership.Password);
            Assert.AreEqual(resultMembership.User.Id, _testMembership.User.Id);
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
                _membershipRepository.Delete(_membershipRepository.Get());
                _userRepository.Delete(_userRepository.Get());
                unitOfWork.Commit();
            }
        }
    }
}
