using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

namespace OctopusStore.Specifications
{
    public abstract class DetailSpecification<T> : BaseSpecification<T>, IDetailSpecification<T> where T : Entity
    {
        public DetailSpecification(int id)
            : base(i => (i.Id == id))
        {   }
    }
}
