using System;
using System.Collections.Generic;
using System.Linq;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Logic.Tests.Fakes
{
    class FakeNewsTextRepository : FakeRepositoryBase<NewsText>, INewsTextRepository
    {
        public FakeNewsTextRepository(FakeUnitOfWorkFactory fakeUnitOfWorkFactory)
            : base(fakeUnitOfWorkFactory)
        {
        }

        public override IQueryable<NewsText> Get()
        {
            return
                EntityCollection.Select(
                    n => new NewsText { Id = n.Id, Text = n.Text, Language = n.Language})
                    .AsQueryable();
        }

        public IQueryable<NewsText> GetWithNews()
        {
            return base.Get();
        }

        public override NewsText GetById(Guid id)
        {
            return Get().FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<NewsText> GetWithTexts()
        {
            return base.Get();
        }

        protected override List<NewsText> EntityCollection
        {
            get
            {
                return FakeUnitOfWorkFactory.NewsTexts;
            }
        }
    }
}
