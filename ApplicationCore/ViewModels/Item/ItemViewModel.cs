using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace ApplicationCore.ViewModels
{
    public class ItemViewModel: EntityViewModel<Item>
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public int BrandId { get; set; }
        public int MeasurementUnitId { get; set; }
        public string Description { get; set; }

        public ItemViewModel(): base()
        {
        }
        public ItemViewModel(Item item): base(item)
        {
            Title = item.Title;
            CategoryId = item.CategoryId;
            StoreId = item.StoreId;
            BrandId = item.BrandId;
            MeasurementUnitId = item.MeasurementUnitId;
            Description = item.Description;
        }

        public override Item ToModel()
        {
            return new Item()
            {
                Id = Id,
                Title = Title,
                Description = Description,
                CategoryId = CategoryId,
                StoreId = StoreId,
                BrandId = BrandId,
                MeasurementUnitId = MeasurementUnitId
            };
        }
        public override Item UpdateModel(Item modelToUpdate)
        {
            modelToUpdate.Title = Title;
            modelToUpdate.Description = Description;
            return modelToUpdate;
        }
    }
}
