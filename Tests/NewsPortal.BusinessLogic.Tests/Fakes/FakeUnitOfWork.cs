using NewsPortal.Domain.Dal.UnitOfWork;

namespace NewsPortal.Logic.Tests.Fakes
{
    class FakeUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {}

        public void Commit()
        {}
    }
}
