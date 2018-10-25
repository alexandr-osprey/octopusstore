using ApplicationCore.Entities;
using ApplicationCore.Specifications;

namespace ApplicationCore.Specifications
{
    public class StoreIndexSpecification: Specification<Store>
    {
        public StoreIndexSpecification(int pageIndex, int pageSize, string title)
           : base(i => (!HasValue(title) || i.Title.Contains(title)))
        {
            Take = pageSize;
            Skip = Take * (pageIndex - 1);

            Description = $"{nameof(pageIndex)}: {pageIndex}, {nameof(pageSize)}: {pageSize}";
            if (HasValue(title)) Description += $", { nameof(title)}: {title}";
        }
    }
}
