using System;
using System.Collections.Generic;
using System.Linq;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Logic.Tests.Fakes
{
    class FakeNewsRepository : FakeRepositoryBase<News>, INewsRepository
    {
        public FakeNewsRepository(FakeUnitOfWorkFactory fakeUnitOfWorkFactory)
            : base(fakeUnitOfWorkFactory)
        {
        }

        public override IQueryable<News> Get()
        {
            return
                EntityCollection.Select(
                    n => new News { Id = n.Id, Date = n.Date, AllowAnonymous = n.AllowAnonymous })
                    .AsQueryable();
        }

        public override News GetById(Guid id)
        {
            return Get().FirstOrDefault(m => m.Id == id);
        }

        public override void Create(News news)
        {
            foreach (NewsText text in news.NewsTexts)
            {
                text.News = news;
                text.Id = Guid.NewGuid();
                FakeUnitOfWorkFactory.NewsTexts.Add(text);
            }
            base.Create(news);
        }

        public IQueryable<News> GetWithTexts()
        {
            return base.Get();
        }

        protected override List<News> EntityCollection
        {
            get
            {
                return FakeUnitOfWorkFactory.NewsRecords;
            }
        }
    }
}
