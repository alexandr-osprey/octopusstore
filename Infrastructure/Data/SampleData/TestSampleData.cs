using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.SampleData
{
    public class TestSampleData : SampleData
    {
        protected override bool DropBeforeSeed => true; 

        public TestSampleData(StoreContext storeContext, IConfiguration configuration) : base(storeContext, configuration)
        {
        }
    }
}
