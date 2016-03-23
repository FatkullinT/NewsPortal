using System;
using System.Linq;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Security;
using NewsPortal.Security.Tests.Fakes;
using NUnit.Framework;

namespace NewsPortal.Security.Tests
{
    /// <summary>
    /// Тесты провайдера ролей безопасности
    /// </summary>
    [TestFixture]
    public class RoleProviderTests
    {
        private IUserRegistrationService _userRegistrationService;
        private IRoleProvider _roleProvider;
        private FakeUnitOfWorkFactory _unitOfWorkFactory;


        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = new FakeUnitOfWorkFactory();
            IUserRepository userRepository = new FakeUserRepository(_unitOfWorkFactory);
            IMembershipRepository membershipRepository = new FakeMembershipRepository(_unitOfWorkFactory);
            IRoleRepository roleRepository = new FakeRoleRepository(_unitOfWorkFactory);
            _roleProvider = new RoleProvider(roleRepository, _unitOfWorkFactory);
            _userRegistrationService =
                new UserRegistrationService(userRepository, membershipRepository,_unitOfWorkFactory);
            CreateTestData();
        }

        private Guid _registeredUserId;
        private Guid _registeredAdminId;
        private string _registeredAdminRole;
        private string _registeredUserRole;

        private void CreateTestData()
        {
            _registeredUserId = _userRegistrationService.Register("UserName", "VeryHardPassword123");
            _registeredAdminId = _userRegistrationService.Register("AdminName", "VeryHardPassword123");
            _registeredAdminRole = "Admin";
            _registeredUserRole = "User";
            _roleProvider.CreateRole(_registeredAdminRole, false);
            _roleProvider.CreateRole(_registeredUserRole, false);
            _roleProvider.AddUserToRole(_registeredAdminId, _registeredAdminRole);
        }

        /// <summary>
        /// Создание роли безопасности
        /// </summary>
        [Test]
        public void RoleProvider_CreateRoleTest()
        {
            //Arrange
            string testRole = "TestRole";
            //Act
            _roleProvider.CreateRole(testRole, true);
            //Assert
            Assert.IsTrue(_unitOfWorkFactory.Roles.Any(role => string.Equals(role.Name, testRole)));
        }

        /// <summary>
        /// Повторное создание роли безопасности с выводом ошибки
        /// </summary>
        [Test]
        public void RoleProvider_DoubleCreateRoleTestWithExceptionThrow()
        {
            //Act
            //Assert
            Assert.Throws<Exception>(() => _roleProvider.CreateRole(_registeredAdminRole, true));
            Assert.AreEqual(_unitOfWorkFactory.Roles.Count(role => string.Equals(role.Name, _registeredAdminRole)), 1);
        }

        /// <summary>
        /// Повторное создание роли безопасности без вывода ошибки
        /// </summary>
        [Test]
        public void RoleProvider_DoubleCreateRoleTestWithoutExceptionThrow()
        {
            //Act
            _roleProvider.CreateRole(_registeredAdminRole, false);
            //Assert
            Assert.AreEqual(_unitOfWorkFactory.Roles.Count(role => string.Equals(role.Name, _registeredAdminRole)), 1);
        }

        /// <summary>
        /// Имеет ли пользователь роль
        /// </summary>
        [Test]
        public void RoleProvider_IsUserInRoleTest()
        {
            //Act
            bool result = _roleProvider.IsUserInRole(_registeredAdminId, _registeredAdminRole);
            //Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Проверка отсутствия роли у пользователя
        /// </summary>
        [Test]
        public void RoleProvider_IsUserNotInRoleTest()
        {
            //Act
            bool result = _roleProvider.IsUserInRole(_registeredUserId, _registeredAdminRole);
            //Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Получение всех ролей пользователя
        /// </summary>
        [Test]
        public void RoleProvider_GetRolesForUser()
        {
            //Act
            string[] roles = _roleProvider.GetRolesForUser(_registeredAdminId);
            //Assert
            Assert.AreEqual(roles.Length, 1);
            Assert.AreEqual(roles[0], _registeredAdminRole);
        }

        /// <summary>
        /// Добавление роли пользователю
        /// </summary>
        [Test]
        public void RoleProvider_AddUserToRoleTest()
        {
            //Act
            _roleProvider.AddUserToRole(_registeredUserId, _registeredUserRole);
            //Assert
            User user =_unitOfWorkFactory.Users.Single(u => u.Id == _registeredUserId);
            Assert.IsTrue(user.Roles.Any(role => string.Equals(role.Name, _registeredUserRole)));
        }

        /// <summary>
        /// Удаление роли у пользователя
        /// </summary>
        [Test]
        public void RoleProvider_RemoveUserFromRoleTest()
        {
            //Act
            _roleProvider.AddUserToRole(_registeredAdminId, _registeredAdminRole);
            //Assert
            User user = _unitOfWorkFactory.Users.Single(u => u.Id == _registeredAdminId);
            Assert.IsTrue(user.Roles.Any(role => string.Equals(role.Name, _registeredAdminRole)));
        }
    }
}
