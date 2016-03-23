using System;
using System.Linq;
using System.Data.Entity;
using NewsPortal.Dal.Context;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Repository
{
    public class NewsTextRepository : Repository<NewsText, Guid>, INewsTextRepository
    {
        public NewsTextRepository(ContextProvider contextProvider)
            : base(contextProvider)
        {}

        public IQueryable<NewsText> GetWithNews()
        {
            return EntitySet.Include(newsText => newsText.News).AsQueryable();
        }
    }
}