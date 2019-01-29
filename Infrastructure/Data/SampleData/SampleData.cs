namespace Infrastructure.Data.SampleData
{
    public class SampleData
    {
        public StoreContext Context { get; }
        protected virtual bool DropBeforeSeed { get; } = false;

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
        public Orders Orders { get; }
        public OrderItems OrderItems { get; }
        public Users Users { get; }


        public SampleData(StoreContext context)
        {
            Context = context;
            if (DropBeforeSeed)
                Context.Database.EnsureDeleted();
            Users = new Users();
            Brands = new Brands(Context);
            MeasurementUnits = new MeasurementUnits(Context);
            Stores = new Stores(Context);
            Categories = new Categories(Context);
            Characteristics = new Characteristics(Context, Categories);
            CharacteristicValues = new CharacteristicValues(Context, Characteristics);
            Items = new Items(Context, Brands, Stores, Categories, MeasurementUnits);
            ItemVariants = new ItemVariants(Context, Items);
            CartItems = new CartItems(Context, ItemVariants);
            ItemProperties = new ItemProperties(Context, ItemVariants, CharacteristicValues);
            ItemImages = new ItemImages(Context, Items);
            Orders = new Orders(Context, Stores);
            OrderItems = new OrderItems(Context, Orders, ItemVariants);
            Orders.RecalculateSums();
        }
    }
}
