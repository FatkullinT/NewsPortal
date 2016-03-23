using System;
using System.Dynamic;
using NewsPortal.Domain.Dal.Repository;

namespace NewsPortal.Domain.Dal.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}