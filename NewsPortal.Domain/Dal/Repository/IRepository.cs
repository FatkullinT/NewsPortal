using System.Collections.Generic;
using System.Linq;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Domain.Dal.Repository
{
    public interface IRepository<T, in I> where T : Entity<I>
    {
        T GetById(I id);

        IQueryable<T> Get();

        void Create(T newRecord);

        void Update(T record, params string[] changedProperties);

        void Delete(IQueryable<T> entities);

        void Delete(T entity);
    }
}