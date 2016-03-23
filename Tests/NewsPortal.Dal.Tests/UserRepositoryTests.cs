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
    public class UserRepositoryTests
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IUserRepository _userRepository;
        private User[] _testUsers;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = DalTestsSetup.UnitOfWorkFactory;
            _userRepository = new UserRepository(DalTestsSetup.ContextProvider);
            CreateTestData();
        }

        /// <summary>
        /// Заполнение тестовыми данными
        /// </summary>
        private void CreateTestData()
        {
            _testUsers = new[]
            {
                new User {Name = "Test0"},
                new User {Name = "Test1"},
                new User {Name = "Test1"}
            };
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                foreach (User testUser in _testUsers)
                {
                    _userRepository.Create(testUser);
                }
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        [Test]
        public void UserRepository_CreateTest()
        {
            //Arange
            User user = new User();
            user.Name = "CreateTest";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Create(user);
                unitOfWork.Commit();
            }
            //Assert
            User resultUser;
            using (_unitOfWorkFactory.Create())
            {
                resultUser = _userRepository.GetById(user.Id);
            }
            Assert.IsNotNull(resultUser);
            Assert.AreEqual(user.Name, resultUser.Name);
        }

        /// <summary>
        /// Изменение записи
        /// </summary>
        [Test]
        public void UserRepository_UpdateTest()
        {
            //Arange
            Guid testUserId = _testUsers[0].Id;
            User user = new User();
            user.Id = testUserId;
            user.Name = "UpdateTest";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Update(user, "Name");
                unitOfWork.Commit();
            }
            //Assert
            User resultUser;
            using (_unitOfWorkFactory.Create())
            {
                resultUser = _userRepository.GetById(testUserId);
            }
            Assert.IsNotNull(resultUser);
            Assert.AreEqual(user.Name, resultUser.Name);
            Assert.AreEqual(user.Id, testUserId);
        }

        /// <summary>
        /// Поиск записи по id
        /// </summary>
        [Test]
        public void UserRepository_GeByIdTest()
        {
            //Arange
            User userTemplate = _testUsers.First();
            User result;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                result = _userRepository.GetById(userTemplate.Id);
            }
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userTemplate.Name, result.Name);
        }

        /// <summary>
        /// Поиск нескольких записей по имени
        /// </summary>
        [Test]
        public void UserRepository_FindByNameTest()
        {
            //Arange
            User firstUserTemplate = _testUsers[1];
            User secondUserTemplate = _testUsers[2];
            User[] resultUsers;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                resultUsers = _userRepository.Get().Where(user => user.Name == "Test1").ToArray();
            }
            //Assert
            Assert.IsNotNull(resultUsers);
            Assert.AreEqual(resultUsers.Length, 2);
            Assert.IsTrue(resultUsers.Any(user => user.Id == firstUserTemplate.Id));
            Assert.IsTrue(resultUsers.Any(user => user.Id == secondUserTemplate.Id));
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
                _userRepository.Delete(_userRepository.Get());
                unitOfWork.Commit();
            }
        }
    }
}
