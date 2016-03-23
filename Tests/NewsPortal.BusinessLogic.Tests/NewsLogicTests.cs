using System;
using System.Linq;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;
using NewsPortal.Logic.Tests.Fakes;
using NUnit.Framework;

namespace NewsPortal.Logic.Tests
{
    /// <summary>
    /// Тесты логики работы с записями новостей
    /// </summary>
    [TestFixture]
    public class NewsLogicTests
    {
        private INewsLogic _newsLogic;
        private FakeAuthenticationData _authenticationData;
        private FakeUnitOfWorkFactory _unitOfWorkFactory;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = new FakeUnitOfWorkFactory();
            FakeNewsRepository newsRepository = new FakeNewsRepository(_unitOfWorkFactory);
            FakeNewsTextRepository newsTextRepository = new FakeNewsTextRepository(_unitOfWorkFactory);
            _authenticationData = new FakeAuthenticationData();
            _authenticationData.IsAuthenticated = true;
            _authenticationData.IsAdministrator = false;
            _newsLogic = new NewsLogic(newsRepository, newsTextRepository, _unitOfWorkFactory, _authenticationData);
            CreateTestData();
        
        }

        private void CreateTestData()
        {
            for (int i = 1; i <= 50; i++)
            {
                News news = GetNewsRecord(i);
                _newsLogic.SaveNewsRecord(news);
            }
        }

        /// <summary>
        /// Генерация записи новостей
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private News GetNewsRecord(int recordNumber)
        {
            News news = _newsLogic.GetEmptyNewsRecord();
            news.Date = DateTime.Now.AddHours(-recordNumber);
            news.AllowAnonymous = recordNumber % 2 > 0;
            NewsText rusText = news.NewsTexts.First(text => text.Language == Language.Rus);
            rusText.Text = "rustest";
            NewsText engText = news.NewsTexts.First(text => text.Language == Language.Eng);
            engText.Text = "engtest";
            return news;
        }

        /// <summary>
        /// Получение страницы записей
        /// </summary>
        [Test]
        public void NewsLogic_GetPage()
        {
            //Act
            NewsPage page = _newsLogic.GetNewsPage(1, Language.Rus);
            //Assert
            Assert.AreEqual(page.Language, Language.Rus);
            Assert.AreEqual(page.Count, 20);
            Assert.IsTrue(page.MoreRecords);
            Assert.IsTrue(page.All(record=>record.Language == Language.Rus));
            Assert.IsFalse(page.All(record => record.News.AllowAnonymous));
        }

        /// <summary>
        /// Получение страницы записей без входа в систему
        /// </summary>
        [Test]
        public void NewsLogic_GetPageAnonymous()
        {
            //Arange
            _authenticationData.IsAuthenticated = false;
            //Act
            NewsPage page = _newsLogic.GetNewsPage(1, Language.Rus);
            //Assert
            Assert.AreEqual(page.Language, Language.Rus);
            Assert.AreEqual(page.Count, 20);
            Assert.IsTrue(page.MoreRecords);
            Assert.IsTrue(page.All(record => record.Language == Language.Rus));
            Assert.IsTrue(page.All(record => record.News.AllowAnonymous));
        }

        /// <summary>
        /// Изменение записи новости
        /// </summary>
        [Test]
        public void NewsLogic_UpdateRecord()
        {
            //Arange
            News news = _unitOfWorkFactory.NewsRecords.First();
            News newsForUpdate = _newsLogic.GetEmptyNewsRecord();
            newsForUpdate.Id = news.Id;
            NewsText rusText = news.NewsTexts.First(text => text.Language == Language.Rus);
            NewsText rusTextOnUpdate = news.NewsTexts.First(text => text.Language == Language.Rus);
            NewsText engText = news.NewsTexts.First(text => text.Language == Language.Eng);
            NewsText engTextOnUpdate = news.NewsTexts.First(text => text.Language == Language.Eng);
            rusTextOnUpdate.Id = rusText.Id;
            rusTextOnUpdate.Text = "rus updated";
            engTextOnUpdate.Id = engText.Id;
            engTextOnUpdate.Text = "eng updated";
            //Act
            _newsLogic.SaveNewsRecord(news);
            //Assert
            Assert.AreEqual(_unitOfWorkFactory.NewsRecords.Count, 50);
            Assert.AreEqual(_unitOfWorkFactory.NewsTexts.Count, 100);
            NewsText textResult = _unitOfWorkFactory.NewsTexts.First(text => text.Id == rusText.Id);
            Assert.AreEqual(textResult.Text, "rus updated");
        }

        /// <summary>
        /// Создание записи новости
        /// </summary>
        [Test]
        public void NewsLogic_CreateRecord()
        {
            //Arange
            News news = GetNewsRecord(0);
            //Act
            _newsLogic.SaveNewsRecord(news);
            //Assert
            Assert.AreEqual(_unitOfWorkFactory.NewsRecords.Count, 51);
            Assert.AreEqual(_unitOfWorkFactory.NewsTexts.Count, 102);
        }

        /// <summary>
        /// Удаление (деактивация) записи новости
        /// </summary>
        [Test]
        public void NewsLogic_DeactivateRecord()
        {
            //Arange
            News news = _unitOfWorkFactory.NewsRecords.First();
            //Act
            _newsLogic.DeactivateNewsRecord(news.Id);
            //Assert
            News resultNews = _unitOfWorkFactory.NewsRecords.First(n => n.Id == news.Id);
            Assert.IsFalse(resultNews.IsActive);
        }
    }
}
