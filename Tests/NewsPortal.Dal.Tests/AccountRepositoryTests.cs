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
    public class AccountRepositoryTests
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IAccountRepository _accountRepository;
        private Account[] _testAccounts;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = DalTestsSetup.UnitOfWorkFactory;
            _accountRepository = new AccountRepository(DalTestsSetup.ContextProvider);
            CreateTestData();
        }

        /// <summary>
        /// Заполнение тестовыми данными
        /// </summary>
        private void CreateTestData()
        {
            _testAccounts = new[]
            {
                new Account {Email = "test1@mail.ru", SendNews = false, UserId = Guid.NewGuid()},
                new Account {Email = "test2@mail.ru", SendNews = true, UserId = Guid.NewGuid()},
                new Account {Email = "test3@mail.ru", SendNews = true, UserId = Guid.NewGuid()}
            };
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                foreach (Account testUserData in _testAccounts)
                {
                    _accountRepository.Create(testUserData);
                }
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        [Test]
        public void AccountRepository_CreateTest()
        {
            //Arange
            Guid userId = Guid.NewGuid();
            Account account = new Account();
            account.Email = "CreateTest@mail.ru";
            account.SendNews = false;
            account.UserId = userId;
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _accountRepository.Create(account);
                unitOfWork.Commit();
            }
            //Assert
            Account resultAccount;
            using (_unitOfWorkFactory.Create())
            {
                resultAccount = _accountRepository.GetById(account.Id);
            }
            Assert.IsNotNull(resultAccount);
            Assert.AreEqual(resultAccount.Email, account.Email);
            Assert.AreEqual(resultAccount.SendNews, account.SendNews);
            Assert.AreEqual(resultAccount.UserId, userId);
        }

        /// <summary>
        /// Изменение записи
        /// </summary>
        [Test]
        public void AccountRepository_UpdateTest()
        {
            //Arange
            Account accountTemaplate = _testAccounts[0];
            Account account = new Account();
            account.Id = accountTemaplate.Id;
            account.Email = "UpdateTest@mail.ru";
            account.SendNews = true;
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _accountRepository.Update(account, "Email", "SendNews");
                unitOfWork.Commit();
            }
            //Assert
            Account resultAccount;
            using (_unitOfWorkFactory.Create())
            {
                resultAccount = _accountRepository.GetById(accountTemaplate.Id);
            }
            Assert.IsNotNull(resultAccount);
            Assert.AreEqual(resultAccount.Email, account.Email);
            Assert.AreEqual(resultAccount.SendNews, account.SendNews);
            Assert.AreEqual(resultAccount.UserId, accountTemaplate.UserId);
        }

        /// <summary>
        /// Поиск записи по id пользователя
        /// </summary>
        [Test]
        public void AccountRepository_GeByIdTest()
        {
            //Arange
            Account accountTemplate = _testAccounts.First();
            Account resultAccount;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                resultAccount = _accountRepository.Get().SingleOrDefault(ud => ud.UserId == accountTemplate.UserId);
            }
            //Assert
            Assert.IsNotNull(resultAccount);
            Assert.AreEqual(resultAccount.Email, resultAccount.Email);
            Assert.AreEqual(resultAccount.SendNews, resultAccount.SendNews);
            Assert.AreEqual(resultAccount.Id, resultAccount.Id);
        }

        /// <summary>
        /// Поиск всех email для рассылки
        /// </summary>
        [Test]
        public void AccountRepository_FindEmailsForSending()
        {
            //Arange
            string[] resultEmails;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                resultEmails = _accountRepository.Get().Where(ud => ud.SendNews).Select(ud=>ud.Email).ToArray();
            }
            //Assert
            Assert.IsNotNull(resultEmails);
            Assert.AreEqual(resultEmails.Length, 2);
            Assert.IsTrue(resultEmails.Contains(_testAccounts[1].Email));
            Assert.IsTrue(resultEmails.Contains(_testAccounts[2].Email));
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
                _accountRepository.Delete(_accountRepository.Get());
                unitOfWork.Commit();
            }
        }
    }
}
