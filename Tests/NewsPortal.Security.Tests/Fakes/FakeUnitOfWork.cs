using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Domain.Dal.UnitOfWork;

namespace NewsPortal.Security.Tests.Fakes
{
    class FakeUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {}

        public void Commit()
        {}
    }
}
