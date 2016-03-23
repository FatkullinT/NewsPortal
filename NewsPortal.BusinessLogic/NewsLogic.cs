using System;
using System.Linq;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;
using NewsPortal.Domain.Logic;

namespace NewsPortal.Logic
{
    /// <summary>
    /// Логика работы с записями новостей
    /// </summary>
    public class NewsLogic : INewsLogic
    {
        private readonly INewsRepository _newsRepository;
        private readonly INewsTextRepository _newsTextRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IAuthenticationData _authenticationData;

        /// <summary>
        /// Максимальное количество записей на странице
        /// </summary>
        private const int PageRecordsCount = 20;

        public NewsLogic(
            INewsRepository newsRepository, 
            INewsTextRepository newsTextRepository, 
            IUnitOfWorkFactory unitOfWorkFactory,
            IAuthenticationData authenticationData)
        {
            _newsRepository = newsRepository;
            _newsTextRepository = newsTextRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _authenticationData = authenticationData;
        }

        /// <summary>
        /// Получение страницы новостей
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="language">Язык</param>
        /// <returns></returns>
        public NewsPage GetNewsPage(int pageNumber, Language language)
        {
            NewsPage response = new NewsPage();
            using (_unitOfWorkFactory.Create())
            {
                NewsText[] newsTexts =
                    _newsTextRepository
                    .GetWithNews()
                    .Where(text=>
                        text.Language == language && 
                        text.News.IsActive && 
                        (_authenticationData.IsAuthenticated || text.News.AllowAnonymous))
                    .OrderByDescending(text=>text.News.Date)
                    .Skip((pageNumber - 1)*PageRecordsCount)
                    .Take(PageRecordsCount + 1)
                    .ToArray();
                response.Language = language;
                response.PageNumber = pageNumber;
                response.MoreRecords = newsTexts.Length > PageRecordsCount;
                response.AddRange(newsTexts.Take(PageRecordsCount));
            }
            return response;
        }

        /// <summary>
        /// Получение записи новостей с текстами на всех языках
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public News GetNewsRecord(Guid newsId)
        {
            using (_unitOfWorkFactory.Create())
            {
                return _newsRepository.GetWithTexts().First(record => record.Id == newsId);
            }
        }

        /// <summary>
        /// Получение новой записи новостей
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public News GetEmptyNewsRecord()
        {
            News newsRecord = new News
            {
                Date = DateTime.Now,
                AllowAnonymous = true,
                NewsTexts = new[]
                {
                    new NewsText() {Language = Language.Eng},
                    new NewsText() {Language = Language.Rus}
                }
            };
            return newsRecord;
        }

        /// <summary>
        /// Сохранение записи новостей
        /// </summary>
        /// <param name="news"></param>
        public void SaveNewsRecord(News news)
        {
            if (news.Id == Guid.Empty)
            {
                CreateNewsRecord(news);
            }
            else
            {
                UpdateNewsRecord(news);
            }
        }

        /// <summary>
        /// Создание записи новости с текстами
        /// </summary>
        /// <param name="news"></param>
        private void CreateNewsRecord(News news)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                news.IsActive = true;
                _newsRepository.Create(news);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Обновление записи новости и ее текстов
        /// </summary>
        /// <param name="news"></param>
        private void UpdateNewsRecord(News news)
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _newsRepository.Update(news, "AllowAnonymous");
                foreach (NewsText newsText in news.NewsTexts)
                {
                     _newsTextRepository.Update(newsText, "Text");
                }
                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// Деактивация записи новости
        /// </summary>
        /// <param name="newsId"></param>
        public void DeactivateNewsRecord(Guid newsId)
        {
            News news = new News()
            {
                Id = newsId,
                IsActive = false
            };
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                _newsRepository.Update(news, "IsActive");
                unitOfWork.Commit();
            }
        }
    }
}
