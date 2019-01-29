using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public abstract class SampleDataEntities<T> where T: Entity, IGenericMemberwiseClonable<T>
    {
        protected StoreContext Context { get; }
        protected abstract IEnumerable<T> GetSourceEntities();

        protected List<T> entities = new List<T>();
        public List<T> Entities { get => entities; }

        public SampleDataEntities(StoreContext storeContext)
        {
            Context = storeContext;
        }

        protected void Seed()
        {
            if (!Context.Set<T>().Any())
            {
                BeforeSeed();
                var savingEntities = GetSourceEntities();
                Context.AddRange(savingEntities);
                Context.SaveChanges();
                AfterSeed(savingEntities);
            }
        }

        public virtual void Init()
        {
            entities = GetQueryable().ToList();
        }

        protected virtual void BeforeSeed()
        {
        }

        protected virtual void AfterSeed(IEnumerable<T> entities)
        {
        }

        protected virtual IQueryable<T> GetQueryable()
        {
            return Context.Set<T>();
        }
    }
}
