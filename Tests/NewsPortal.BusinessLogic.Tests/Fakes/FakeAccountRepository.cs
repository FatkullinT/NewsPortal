using System;
using System.Collections.Generic;
using System.Linq;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Logic.Tests.Fakes
{
    class FakeAccountRepository : FakeRepositoryBase<Account>, IAccountRepository
    {
        public FakeAccountRepository(FakeUnitOfWorkFactory fakeUnitOfWorkFactory)
            : base(fakeUnitOfWorkFactory)
        {
        }

        protected override List<Account> EntityCollection
        {
            get
            {
                return FakeUnitOfWorkFactory.UserDataRecords;
            }
        }
    }
}
