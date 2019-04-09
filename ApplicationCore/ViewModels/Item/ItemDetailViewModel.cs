using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemDetailViewModel: ItemViewModel
    {
        public CategoryViewModel Category { get; set; }
        public StoreViewModel Store { get; set; }
        public BrandViewModel Brand { get; set; }
        public MeasurementUnitViewModel MeasurementUnit { get; set; }
        public IEnumerable<ItemVariantViewModel> ItemVariants { get; set; }
        //public IEnumerable<ItemVariantImageViewModel> Images { get; set; }

        public ItemDetailViewModel(Item item): base(item)
        {
            Category = new CategoryViewModel(item.Category);
            Store = new StoreViewModel(item.Store);
            Brand = new BrandViewModel(item.Brand);
            MeasurementUnit = new MeasurementUnitViewModel(item.MeasurementUnit);
            ItemVariants = (from itemVariant in item.ItemVariants select new ItemVariantViewModel(itemVariant));
            //Images = item.ItemVariants.SelectMany(v => v.Images).Select(i => new ItemVariantImageViewModel(i));
        }
    }
}
