using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public abstract class SampleDataEntities<T> where T: Entity, IGenericMemberwiseClonable<T>
    {
        protected readonly StoreContext _storeContext;
        protected abstract IEnumerable<T> GetSourceEntities();

        protected List<T> _entities = new List<T>();
        public List<T> Entities { get => _entities; }

        public SampleDataEntities(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        protected void Seed()
        {
            BeforeSeed();
            _storeContext.Set<T>().RemoveRange(_storeContext.Set<T>());
            _storeContext.SaveChanges();
            var savingEntities = GetSourceEntities();
            _storeContext.AddRange(savingEntities);
            _storeContext.SaveChanges();
            AfterSeed(savingEntities.ToList());
            _entities = _storeContext.Set<T>().AsNoTracking().ToList();
        }

        protected virtual void BeforeSeed()
        {

        }
        protected virtual void AfterSeed(List<T> entities)
        {
            
        }
    }
}
