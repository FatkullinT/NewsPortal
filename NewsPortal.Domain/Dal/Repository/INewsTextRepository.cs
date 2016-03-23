using System;
using System.Linq;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Dal.Repository
{
    public interface INewsTextRepository : IRepository<NewsText, Guid>
    {
        IQueryable<NewsText> GetWithNews();
    }
}