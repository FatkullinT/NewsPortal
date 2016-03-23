using NewsPortal.Dal.Context;
using NewsPortal.Dal.Repository;
using NewsPortal.Domain;
using NewsPortal.Domain.Dal;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dal.UnitOfWork;

namespace NewsPortal.Dal.UnitOfWork
{
    class UnitOfWork : IUnitOfWork
    {
        private ContextProvider _contextProvider;

        public UnitOfWork(ContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _contextProvider.CreateContext();
        }

        public void Commit()
        {
            _contextProvider.Context.SaveChanges();
        }

        public void Dispose()
        {
            if (_contextProvider != null)
            {
                _contextProvider.DisposeContext();
                _contextProvider = null;
            }
        }
    }
}
