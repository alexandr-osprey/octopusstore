using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public abstract class SampleDataEntities<T> where T: Entity, IGenericMemberwiseClonable<T>
    {
        protected StoreContext StoreContext { get; }
        protected abstract IEnumerable<T> GetSourceEntities();

        protected List<T> entities = new List<T>();
        public List<T> Entities { get => entities; }

        public SampleDataEntities(StoreContext storeContext)
        {
            StoreContext = storeContext;
        }

        protected void Seed()
        {
            if (!StoreContext.Set<T>().Any())
            {
                BeforeSeed();
                var savingEntities = GetSourceEntities();
                StoreContext.AddRange(savingEntities);
                StoreContext.SaveChanges();
                AfterSeed(savingEntities.ToList());
            }
            entities = StoreContext.Set<T>().AsNoTracking().ToList();
        }

        protected virtual void BeforeSeed()
        {
        }
        protected virtual void AfterSeed(List<T> entities)
        {
        }
    }
}
