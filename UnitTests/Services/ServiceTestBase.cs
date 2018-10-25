using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Xunit.Abstractions;

namespace UnitTests
{
    public abstract class ServiceTestBase<TEntity, TService>
       : TestBase<TEntity>
        where TService: IService<TEntity>
        where TEntity: Entity
    {
        protected TService service;

        public ServiceTestBase(ITestOutputHelper output)
           : base(output)
        {
            service = Resolve<TService>();
        }
    }
}
