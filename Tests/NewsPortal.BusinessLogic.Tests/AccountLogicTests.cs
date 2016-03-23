using System;
using System.Linq;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;
using NewsPortal.Logic.Tests.Fakes;
using NUnit.Framework;

namespace NewsPortal.Logic.Tests
{
    /// <summary>
    /// Тесты логики работы с данными пользователя
    /// </summary>
    [TestFixture]
    public class AccountLogicTests
    {
        private IAccountLogic _accountLogic;
        private FakeAuthenticationData _authenticationData;
        private FakeUnitOfWorkFactory _unitOfWorkFactory;
        private Account _testAccount;
        private Guid _userId;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = new FakeUnitOfWorkFactory();
            FakeAccountRepository accountRepository = new FakeAccountRepository(_unitOfWorkFactory);
            _authenticationData = new FakeAuthenticationData();
            _accountLogic = new AccountLogic(_authenticationData, _unitOfWorkFactory, accountRepository);
            CreateTestData();
        }

        private void CreateTestData()
        {
            _userId = Guid.NewGuid();
            _authenticationData.CurrentUserId = _userId;
            _authenticationData.IsAuthenticated = true;
            _testAccount = new Account();
            _testAccount.Email = "test@mail.ru";
            _testAccount.SendNews = false;
            _testAccount.UserId = _userId;
            _testAccount.Id = Guid.NewGuid();
            _unitOfWorkFactory.UserDataRecords.Add(_testAccount);
        }

        /// <summary>
        /// Создание записи данных учетной записи
        /// </summary>
        [Test]
        public void AccountLogic_CreateAccount()
        {
            //Arange
            Guid newUserId = Guid.NewGuid();
            _authenticationData.CurrentUserId = newUserId;
            Account account = new Account();
            account.Email = "test2@mail.ru";
            account.SendNews = true;
            //Act
            _accountLogic.SaveAccount(account);
            //Assert
            Assert.AreEqual(_unitOfWorkFactory.UserDataRecords.Count, 2);
            Account resultAccount = _unitOfWorkFactory.UserDataRecords.FirstOrDefault(ud => ud.UserId == newUserId);
            Assert.NotNull(resultAccount);
            Assert.AreEqual(resultAccount.Email, "test2@mail.ru");
        }

        /// <summary>
        /// Обновление записи данных учетной записи
        /// </summary>
        [Test]
        public void AccountLogic_UpdateAccount()
        {
            //Arange
            Account account = new Account();
            account.Email = "test2@mail.ru";
            account.SendNews = true;
            //Act
            _accountLogic.SaveAccount(account);
            //Assert
            Assert.AreEqual(_unitOfWorkFactory.UserDataRecords.Count, 1);
            Account resultAccount = _unitOfWorkFactory.UserDataRecords.First();
            Assert.AreEqual(resultAccount.Email, "test2@mail.ru");
        }

        /// <summary>
        /// Получение записи данных учетной записи
        /// </summary>
        [Test]
        public void AccountLogic_GetAccount()
        {
            //Act
            Account account = _accountLogic.GetAccount();
            //Assert
            Assert.NotNull(account);
            Assert.AreEqual(account.Email, _testAccount.Email);
        }
    }
}
