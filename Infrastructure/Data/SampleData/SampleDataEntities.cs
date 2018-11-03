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
            if (!_storeContext.Set<T>().Any())
            {
                _storeContext.AddRange(GetSourceEntities());
                _storeContext.SaveChanges();
            }
            _entities = _storeContext.Set<T>().AsNoTracking().ToList();
        }
    }
}
