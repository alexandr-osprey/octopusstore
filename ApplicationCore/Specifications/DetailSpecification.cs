using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    /// <summary>
    /// Contains full and related information, required to execute a request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DetailSpecification<T>: EntitySpecification<T> where T: Entity
    {
        public DetailSpecification(int id): base(id)
        {
        }
    }
}
