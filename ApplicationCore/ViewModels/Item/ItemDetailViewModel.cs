using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemDetailViewModel : EntityViewModel<Item>
    {
        public string Title { get; set; }
        public CategoryViewModel Category { get; set; }
        public StoreViewModel Store { get; set; }
        public BrandViewModel Brand { get; set; }
        public MeasurementUnitViewModel MeasurementUnit { get; set; }
        public string Description { get; set; }
        public IEnumerable<ItemVariantViewModel> ItemVariants { get; set; }
        public IEnumerable<ItemImageViewModel> Images { get; set; }

        public ItemDetailViewModel(Item item)
            : base(item)
        {
            Title = item.Title;
            Description = item.Description;
            Category = new CategoryViewModel(item.Category);
            Store = new StoreViewModel(item.Store);
            Brand = new BrandViewModel(item.Brand);
            MeasurementUnit = new MeasurementUnitViewModel(item.MeasurementUnit);
            ItemVariants = (from itemVariant in item.ItemVariants select new ItemVariantViewModel(itemVariant));
            Images = (from image in item.Images select new ItemImageViewModel(image));
        }

        public override Item ToModel()
        {
            return new Item()
            {
                Id = Id,
                Title = Title,
                Description = Description,
                CategoryId = Category.Id,
                StoreId = Store.Id,
                BrandId = Brand.Id,
                MeasurementUnitId = MeasurementUnit.Id
            };
        }
        public override Item UpdateModel(Item modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.MeasurementUnitId = MeasurementUnit.Id;
            modelToUpdate.CategoryId = Category.Id;
            modelToUpdate.BrandId = Brand.Id;
            modelToUpdate.StoreId = Store.Id;
            modelToUpdate.Description = Description;
            return modelToUpdate;
        }
    }
}
