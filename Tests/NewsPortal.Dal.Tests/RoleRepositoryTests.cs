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
    public class RoleRepositoryTests
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private Role _freeTestRole;
        private Role _testRoleWithUser;
        private User _freeTestUser;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = DalTestsSetup.UnitOfWorkFactory;
            _roleRepository = new RoleRepository(DalTestsSetup.ContextProvider);
            _userRepository = new UserRepository(DalTestsSetup.ContextProvider);
            CreateTestData();
        }

        /// <summary>
        /// Заполнение тестовыми данными
        /// </summary>
        private void CreateTestData()
        {
            _freeTestUser = new User(){Name = "TestUser1"};
            _freeTestRole = new Role {Name = "TestRole1"};
             _testRoleWithUser = new Role {Name = "TestRole2", Users = new List<User>{new User(){Name = "TestUser2"}}};
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _userRepository.Create(_freeTestUser);
                _roleRepository.Create(_freeTestRole);
                _roleRepository.Create(_testRoleWithUser);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        [Test]
        public void RoleRepository_CreateTest()
        {
            //Arange
            Role role = new Role();
            role.Name = "CreateRoleTest";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _roleRepository.Create(role);
                unitOfWork.Commit();
            }
            //Assert
            Role resultRole;
            using (_unitOfWorkFactory.Create())
            {
                resultRole = _roleRepository.GetById(role.Id);
            }
            Assert.IsNotNull(resultRole);
            Assert.AreEqual(role.Name, resultRole.Name);
        }

        /// <summary>
        /// Изменение записи
        /// </summary>
        [Test]
        public void RoleRepository_UpdateTest()
        {
            //Arange
            Guid testRoleId = _testRoleWithUser.Id;
            Role role = new Role();
            role.Id = testRoleId;
            role.Name = "UpdateRoleTest";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _roleRepository.Update(role, "Name");
                unitOfWork.Commit();
            }
            //Assert
            Role resultRole;
            using (_unitOfWorkFactory.Create())
            {
                resultRole = _roleRepository.GetById(testRoleId);
            }
            Assert.IsNotNull(resultRole);
            Assert.AreEqual(role.Name, resultRole.Name);
            Assert.AreEqual(role.Id, testRoleId);
        }


        /// <summary>
        /// Поиск по названию
        /// </summary>
        [Test]
        public void RoleRepository_FindByNameTest()
        {
            //Arange
            Role roleTemplate = _freeTestRole;
            Role resultRole;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                resultRole = _roleRepository.Get().FirstOrDefault(r => r.Name == roleTemplate.Name);
            }
            //Assert
            Assert.IsNotNull(resultRole);
            Assert.AreEqual(resultRole.Name, resultRole.Name);
            Assert.AreEqual(resultRole.Id, roleTemplate.Id);
        }

        /// <summary>
        /// Создание связи с пользователем
        /// </summary>
        [Test]
        public void RoleRepository_AssociateWithUserTest()
        {
            //Arange
            Guid userId = _freeTestUser.Id;
            Role role = _freeTestRole;
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _roleRepository.AssociateWithUser(role, userId);
                unitOfWork.Commit();
            }
            //Assert
            Role resultRole;
            using (_unitOfWorkFactory.Create())
            {
                resultRole = _userRepository.Get().Where(u => u.Id == userId).SelectMany(u => u.Roles).FirstOrDefault();
            }
            Assert.IsNotNull(resultRole);
            Assert.AreEqual(resultRole.Name, resultRole.Name);
            Assert.AreEqual(resultRole.Id, resultRole.Id);
        }

        /// <summary>
        /// Удаление связи с пользователем
        /// </summary>
        [Test]
        public void RoleRepository_DisassociateWithUserTest()
        {
            //Arange
            Guid userId = _testRoleWithUser.Users.First().Id;
            Role role = new Role() {Id = _testRoleWithUser.Id};
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _roleRepository.DisassociateWithUser(role, userId);
                unitOfWork.Commit();
            }
            //Assert
            Role resultRole;
            using (_unitOfWorkFactory.Create())
            {
                resultRole = _userRepository.Get().Where(u => u.Id == userId).SelectMany(u => u.Roles).FirstOrDefault();
            }
            Assert.IsNull(resultRole);
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
                _roleRepository.Delete(_roleRepository.Get());
                _userRepository.Delete(_userRepository.Get());
                unitOfWork.Commit();
            }
        }
    }
}
