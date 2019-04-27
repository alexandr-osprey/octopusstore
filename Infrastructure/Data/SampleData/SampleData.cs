

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.SampleData
{
    public class SampleData
    {
        public StoreContext Context { get; }
        protected virtual bool DropBeforeSeed { get; } = false;

        public Brands Brands { get; }
        public Stores Stores { get; }
        public Categories Categories { get; }
        public Characteristics Characteristics { get; }
        public CharacteristicValues CharacteristicValues { get; }
        public Items Items { get; }
        public ItemVariants ItemVariants { get; }
        public CartItems CartItems { get; }
        public ItemProperties ItemProperties { get; }
        public ItemVariantImages ItemVariantImages { get; }
        public Orders Orders { get; }
        public Users Users { get; }


        public SampleData(StoreContext context, IConfiguration configuration)
        {
            Context = context;
            if (DropBeforeSeed)
                Context.Database.EnsureDeleted();
            Users = new Users();
            Brands = new Brands(Context);
            Stores = new Stores(Context);
            Categories = new Categories(Context);
            Characteristics = new Characteristics(Context, Categories);
            CharacteristicValues = new CharacteristicValues(Context, Characteristics);
            Items = new Items(Context, Brands, Stores, Categories);
            ItemVariants = new ItemVariants(Context, Items);
            CartItems = new CartItems(Context, ItemVariants);
            ItemProperties = new ItemProperties(Context, ItemVariants, CharacteristicValues);
            ItemVariantImages = new ItemVariantImages(Context, ItemVariants, Categories, configuration);
            Orders = new Orders(Context, ItemVariants);
        }
    }
}
