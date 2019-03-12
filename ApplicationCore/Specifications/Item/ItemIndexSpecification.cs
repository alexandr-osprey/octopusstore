using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Specifications
{
    public class ItemIndexSpecification: EntitySpecification<Item>
    {
        public ItemIndexSpecification(ItemIndexSpecification itemIndexSpecification): base(itemIndexSpecification)
        {
        }

        public ItemIndexSpecification(int pageIndex, int pageSize, string searchValue, IEnumerable<Category> categories, int? storeId, int? brandId, HashSet<int> characteristicValueIds)
           : base(i => (!HasValue(searchValue) || (i.Title.ContainsCI(searchValue)) || i.Brand.Title.ContainsCI(searchValue)) 
                        && (!(categories.Any()) || categories.Any(c => c.Id == i.CategoryId)) 
                        && (!storeId.HasValue || i.StoreId == storeId) 
                        && (!brandId.HasValue || i.BrandId == brandId) 
                        && (!characteristicValueIds.Any() || 
                            characteristicValueIds.IsSubsetOf(from p in i.ItemVariants.SelectMany(v => v.ItemProperties) select p.CharacteristicValueId))) 
        {
            //Take = pageSize;
            //Skip = Take * (pageIndex - 1);
            SetPaging(pageIndex, pageSize);

            Description = $"{nameof(pageIndex)}: {pageIndex}, {nameof(pageSize)}: {pageSize}" +
                $", {nameof(searchValue)}: {searchValue ?? "null"}" +
                $", {nameof(categories)}: {string.Join(", ", categories)}" +
                $", {nameof(storeId)} (0 - not provided): {storeId ?? 0}" +
                $", {nameof(brandId)} (0 - not provided): {brandId ?? 0}";
        }
    }
}
