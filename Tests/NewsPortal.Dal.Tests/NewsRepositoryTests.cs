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
    public class NewsRepositoryTests
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private INewsRepository _newsRepository;
        private INewsTextRepository _newsTextRepository;
        private News[] _testNews;


        [SetUp]
        public void Setup()
        {
            _unitOfWorkFactory = DalTestsSetup.UnitOfWorkFactory;
            _newsRepository = new NewsRepository(DalTestsSetup.ContextProvider);
            _newsTextRepository = new NewsTextRepository(DalTestsSetup.ContextProvider);
            CreateTestData();
        }

        /// <summary>
        /// Заполнение тестовыми данными
        /// </summary>
        private void CreateTestData()
        {
            _testNews = new[]
            {
                new News
                {
                    AllowAnonymous = true,
                    IsActive = true,
                    Date = new DateTime(2016, 1, 7),
                    NewsTexts = new List<NewsText>
                    {
                        new NewsText {Text = "RusText1", Language = Language.Rus},
                        new NewsText {Text = "EngText1", Language = Language.Eng}
                    }
                },
                new News
                {
                    AllowAnonymous = true,
                    IsActive = false,
                    Date = new DateTime(2016, 1, 5),
                    NewsTexts = new List<NewsText>
                    {
                        new NewsText {Text = "RusText2", Language = Language.Rus},
                        new NewsText {Text = "EngText2", Language = Language.Eng}
                    }
                },
                new News
                {
                    AllowAnonymous = true,
                    IsActive = true,
                    Date = new DateTime(2016, 1, 11),
                    NewsTexts = new List<NewsText>
                    {
                        new NewsText {Text = "RusText3", Language = Language.Rus},
                        new NewsText {Text = "EngText3", Language = Language.Eng}
                    }
                },
                new News
                {
                    AllowAnonymous = true,
                    IsActive = true,
                    Date = new DateTime(2016, 1, 10),
                    NewsTexts = new List<NewsText>
                    {
                        new NewsText {Text = "RusText4", Language = Language.Rus},
                        new NewsText {Text = "EngText4", Language = Language.Eng}
                    }
                }
            };
            foreach (News news in _testNews)
            {
                foreach (NewsText text in news.NewsTexts)
                {
                    text.News = news;
                }
            }
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                foreach (News testNews in _testNews)
                {
                    _newsRepository.Create(testNews);
                }
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        [Test]
        public void NewsRepository_CreateTest()
        {
            //Arange
            NewsText engNewsText = new NewsText();
            engNewsText.Language = Language.Eng;
            engNewsText.Text = "TestNewsTextEng";
            NewsText rusNewsText = new NewsText();
            rusNewsText.Language = Language.Rus;
            rusNewsText.Text = "TestNewsTextRus";
            News news = new News();
            news.Date = DateTime.Today;
            news.IsActive = true;
            news.AllowAnonymous = true;
            news.NewsTexts = new List<NewsText>() { engNewsText, rusNewsText };
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _newsRepository.Create(news);
                unitOfWork.Commit();
            }
            //Assert
            News resultNews;
            using (_unitOfWorkFactory.Create())
            {
                resultNews = _newsRepository.GetWithTexts().FirstOrDefault(n => n.Id == news.Id);
            }
            Assert.IsNotNull(resultNews);
            Assert.AreEqual(resultNews.Date, news.Date);
            Assert.IsNotNull(resultNews.NewsTexts);
            Assert.AreEqual(resultNews.NewsTexts.Count, 2);
            Assert.IsTrue(resultNews.NewsTexts.Any(text => string.Equals(text.Text, "TestNewsTextEng") && text.Language == Language.Eng));
            Assert.IsTrue(resultNews.NewsTexts.Any(text => string.Equals(text.Text, "TestNewsTextRus") && text.Language == Language.Rus));
        }

        /// <summary>
        /// Изменение записи
        /// </summary>
        [Test]
        public void NewsRepository_UpdateTest()
        {
            //Arange
            News testNews = _testNews[0];
            NewsText engTestNewsText = testNews.NewsTexts.Single(t => t.Language == Language.Eng);
            NewsText rusTestNewsText = testNews.NewsTexts.Single(t => t.Language == Language.Rus);
            testNews.AllowAnonymous = false;
            engTestNewsText.Text = "UpdatedEngText";
            rusTestNewsText.Text = "UpdatedRusText";
            //Act
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _newsRepository.Update(testNews, "AllowAnonymous");
                foreach (NewsText newsText in testNews.NewsTexts)
                {
                    _newsTextRepository.Update(newsText, "Text");
                    
                }
                unitOfWork.Commit();
            }
            //Assert
            News resultNews;
            using (_unitOfWorkFactory.Create())
            {
                resultNews = _newsRepository.GetWithTexts().FirstOrDefault(n => n.Id == testNews.Id);
            }
            Assert.IsNotNull(resultNews);
            Assert.AreEqual(resultNews.Date, testNews.Date);
            Assert.AreEqual(resultNews.AllowAnonymous, false);
            Assert.IsNotNull(resultNews.NewsTexts);
            Assert.AreEqual(resultNews.NewsTexts.Count, 2);
            Assert.IsTrue(resultNews.NewsTexts.Any(text => string.Equals(text.Text, "UpdatedEngText") && text.Language == Language.Eng));
            Assert.IsTrue(resultNews.NewsTexts.Any(text => string.Equals(text.Text, "UpdatedRusText") && text.Language == Language.Rus));
        }

        /// <summary>
        /// Поиск записи по id
        /// </summary>
        [Test]
        public void NewsRepository_GeByIdTest()
        {
            //Arange
            News testNews = _testNews[0];
            News result;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                result = _newsRepository.GetWithTexts().FirstOrDefault(n => n.Id == testNews.Id);
            }
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Date, testNews.Date);
            Assert.AreEqual(result.AllowAnonymous, testNews.AllowAnonymous);
            Assert.IsNotNull(result.NewsTexts);
            Assert.AreEqual(result.NewsTexts.Count, 2);
            Assert.IsTrue(result.NewsTexts.Any(text => string.Equals(text.Text, "RusText1") && text.Language == Language.Rus));
            Assert.IsTrue(result.NewsTexts.Any(text => string.Equals(text.Text, "EngText1") && text.Language == Language.Eng));
        }

        /// <summary>
        /// Получение страницы записей
        /// </summary>
        [Test]
        public void NewsRepository_GetPageTest()
        {
            //Arange
            NewsText[] result;
            //Act
            using (_unitOfWorkFactory.Create())
            {
                result =
                    _newsTextRepository.GetWithNews()
                        .Where(text => text.Language == Language.Rus && text.News.AllowAnonymous && text.News.IsActive)
                        .OrderByDescending(text => text.News.Date)
                        .Skip(2)
                        .Take(2)
                        .ToArray();
            }
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Length, 1);
            NewsText resultText = result.Single();
            Assert.AreEqual(resultText.News.Date, new DateTime(2016, 1, 7));
            Assert.AreEqual(resultText.Text, "RusText1");
            Assert.AreEqual(resultText.Language, Language.Rus);
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
                _newsRepository.Delete(_newsRepository.Get());
                _newsTextRepository.Delete(_newsTextRepository.Get());
                unitOfWork.Commit();
            }
        }
    }
}
