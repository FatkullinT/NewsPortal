using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Logic.Tests.Fakes
{
    abstract class FakeRepositoryBase<T>: IRepository<T, Guid> where T : Entity<Guid>
    {
        protected abstract List<T> EntityCollection { get; }

        protected FakeUnitOfWorkFactory FakeUnitOfWorkFactory;

        public FakeRepositoryBase(FakeUnitOfWorkFactory fakeUnitOfWorkFactory)
        {
            FakeUnitOfWorkFactory = fakeUnitOfWorkFactory;
        }

        public virtual T GetById(Guid id)
        {
            return EntityCollection.FirstOrDefault(e => e.Id == id);
        }

        public virtual IQueryable<T> Get()
        {
            return EntityCollection.AsQueryable();
        }

        public virtual void Create(T newRecord)
        {
            newRecord.Id = Guid.NewGuid();
            EntityCollection.Add(newRecord);
        }

        public virtual void Update(T record, params string[] changedProperties)
        {
            T entity = EntityCollection.First(r=>r.Id == record.Id);
            foreach (PropertyInfo property in typeof (T).GetProperties())
            {
                if (changedProperties.Length > 0 && changedProperties.Contains(property.Name))
                {
                    property.SetValue(entity, property.GetValue(record));
                }
            }
        }

        public void Delete(IQueryable<T> entities)
        {
            Guid[] entityIds = entities.Select(e => e.Id).ToArray();
            EntityCollection.RemoveAll(entity => entityIds.Contains(entity.Id));
        }

        public void Delete(T entity)
        {
            EntityCollection.RemoveAll(e => e.Id == entity.Id);
        }
    }
}
