using System;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Logic
{
    public interface INewsLogic
    {
        /// <summary>
        /// Получение страницы новостей
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="language">Язык</param>
        /// <returns></returns>
        NewsPage GetNewsPage(int pageNumber, Language language);

        /// <summary>
        /// Получение записи новостей с текстами на всех языках
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        News GetNewsRecord(Guid newsId);

        /// <summary>
        /// Получение новой записи новостей
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        News GetEmptyNewsRecord();

        /// <summary>
        /// Сохранение записи новости
        /// </summary>
        /// <param name="news"></param>
        void SaveNewsRecord(News news);

        /// <summary>
        /// Деактивация записи новости
        /// </summary>
        /// <param name="newsId"></param>
        void DeactivateNewsRecord(Guid newsId);
    }
}