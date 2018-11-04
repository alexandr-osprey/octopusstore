namespace Infrastructure.Data.SampleData
{
    public class TestSampleData : SampleData
    {
        protected override bool DropBeforeSeed => true; 

        public TestSampleData(StoreContext storeContext) : base(storeContext)
        {
        }
    }
}
