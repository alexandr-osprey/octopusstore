namespace Infrastructure.Data.SampleData
{
    public class SampleData
    {
        protected readonly StoreContext _storeContext;

        public Brands Brands { get; }
        public MeasurementUnits MeasurementUnits { get; }
        public Stores Stores { get; }
        public Categories Categories { get; }
        public Characteristics Characteristics { get; }
        public CharacteristicValues CharacteristicValues { get; }
        public Items Items { get; }
        public ItemVariants ItemVariants { get; }
        public CartItems CartItems { get; }
        public ItemProperties ItemProperties { get; }
        public ItemImages ItemImages { get; }


        public SampleData(StoreContext context)
        {
            _storeContext = context;
            Brands = new Brands(_storeContext);
            MeasurementUnits = new MeasurementUnits(_storeContext);
            Stores = new Stores(_storeContext);
            Categories = new Categories(_storeContext);
            Characteristics = new Characteristics(_storeContext, Categories);
            CharacteristicValues = new CharacteristicValues(_storeContext, Characteristics);
            Items = new Items(_storeContext, Brands, Stores, Categories, MeasurementUnits);
            ItemVariants = new ItemVariants(_storeContext, Items);
            CartItems = new CartItems(_storeContext, ItemVariants);
            ItemProperties = new ItemProperties(_storeContext, ItemVariants, CharacteristicValues);
            ItemImages = new ItemImages(_storeContext, Items);
        }
    }
}
