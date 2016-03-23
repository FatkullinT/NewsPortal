using NewsPortal.Dal.Context;
using NewsPortal.Domain;
using NewsPortal.Domain.Dal.UnitOfWork;

namespace NewsPortal.Dal.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private ContextProvider _contextProvider;

        public UnitOfWorkFactory(ContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_contextProvider);
        }
    }
}