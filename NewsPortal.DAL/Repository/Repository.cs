using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using NewsPortal.Dal.Context;
using NewsPortal.Domain.Dal.Repository;
using NewsPortal.Domain.Dto;

namespace NewsPortal.Dal.Repository
{
    /// <summary>
    /// Базовый репозиторий
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <typeparam name="I">Тип первичного ключа</typeparam>
    public class Repository<T, I> : IRepository<T, I> where T : Entity<I>
    {
        protected DbSet<T> EntitySet
        {
            get 
            {
                return Context.Set<T>(); 
            }
        }

        protected Context.Context Context
        {
            get
            {
                return _contextProvider.Context;
            }
        }

        private readonly ContextProvider _contextProvider;
        
        public Repository(ContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public virtual T GetById(I id)
        {
            return EntitySet.Find(id);
        }

        public virtual IQueryable<T> Get()
        {
            return EntitySet.AsQueryable();
        }

        public virtual void Create(T entity)
        {
            EntitySet.Add(entity);
        }

        public virtual void Update(T entity, params string[] changedProperties)
        {
            if (changedProperties.Length == 0)
            {
                EntitySet.AddOrUpdate(entity);
            }
            else
            {
                EntitySet.Attach(entity);
                DbEntityEntry<T> entry = Context.Entry(entity);
                foreach (string changedProperty in changedProperties)
                {
                    entry.Property(changedProperty).IsModified = true;
                }
            }
        }

        public void Delete(IQueryable<T> entities)
        {
            EntitySet.RemoveRange(EntitySet);
        }

        public void Delete(T entity)
        {
            EntitySet.Remove(entity);
        }
    }
}