using System;
using System.Linq;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Dal.Repository
{
    public interface INewsRepository : IRepository<News, Guid>
    {
        IQueryable<News> GetWithTexts();
    }
}