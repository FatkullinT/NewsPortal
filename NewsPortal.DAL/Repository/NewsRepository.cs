using System;
using System.Linq;
using System.Data.Entity;
using NewsPortal.Dal.Context;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Repository
{
    public class NewsRepository : Repository<News, Guid>, INewsRepository
    {
        public NewsRepository(ContextProvider contextProvider)
            : base(contextProvider)
        {}

        public IQueryable<News> GetWithTexts()
        {
            return EntitySet.Include(news => news.NewsTexts).AsQueryable();
        }
    }
}