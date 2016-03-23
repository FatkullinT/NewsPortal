using System.Collections.Generic;
using NewsPortal.Domain.Dal.UnitOfWork;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Logic.Tests.Fakes
{
    class FakeUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly FakeUnitOfWork _fakeUnitOfWork = new FakeUnitOfWork();
        public List<News> NewsRecords = new List<News>();
        public List<NewsText> NewsTexts = new List<NewsText>();
        public List<Account> UserDataRecords = new List<Account>();

        public IUnitOfWork Create()
        {
            return _fakeUnitOfWork;
        }
    }
}
