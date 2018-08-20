using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IDetailSpecification<T> : ISpecification<T> where T : Entity
    {  }
}
