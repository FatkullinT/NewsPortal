using System;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Logic
{
    public interface INewsLogic
    {
        /// <summary>
        /// ��������� �������� ��������
        /// </summary>
        /// <param name="pageNumber">����� ��������</param>
        /// <param name="language">����</param>
        /// <returns></returns>
        NewsPage GetNewsPage(int pageNumber, Language language);

        /// <summary>
        /// ��������� ������ �������� � �������� �� ���� ������
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        News GetNewsRecord(Guid newsId);

        /// <summary>
        /// ��������� ����� ������ ��������
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        News GetEmptyNewsRecord();

        /// <summary>
        /// ���������� ������ �������
        /// </summary>
        /// <param name="news"></param>
        void SaveNewsRecord(News news);

        /// <summary>
        /// ����������� ������ �������
        /// </summary>
        /// <param name="newsId"></param>
        void DeactivateNewsRecord(Guid newsId);
    }
}