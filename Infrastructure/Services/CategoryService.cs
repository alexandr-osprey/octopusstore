using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        public int RootCategoryId { get; set; } = 1;
        private readonly IAsyncRepository<Characteristic> _propertyRepository;
        private readonly IAsyncRepository<CharacteristicValue> _propertyValueRepository;
        private readonly IAsyncRepository<Item> _itemRepository;

        public CategoryService(
            IAsyncRepository<Category> repository,
            IAsyncRepository<Characteristic> propertyRepository,
            IAsyncRepository<CharacteristicValue> propertyValueRepository,
            IAsyncRepository<Item> itemRepository,
            IAppLogger<Service<Category>> logger)
            : base(repository, logger)
        {
            _propertyRepository = propertyRepository;
            _propertyValueRepository = propertyValueRepository;
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Category>> ListHierarchyAsync(ISpecification<Category> spec)
        {
            List<Category> flatCategories = new List<Category>();
            await GetCategoryHierarchyAsync(spec, flatCategories);
            return flatCategories;
        }
        public async Task<IEnumerable<Category>> ListSubcategoriesAsync(ISpecification<Category> spec)
        {
            List<Category> flatCategoreies = new List<Category>();
            var categories = await _repository.ListAsync(spec);
            foreach (var category in categories)
            {
                var categorySpec = new Specification<Category>((s => s.Id == category.Id), (s => s.Subcategories));
                categorySpec.Description = $"Category id={category.Id} includes Subcategories";
                await GetCategorySubcategoriesAsync(categorySpec, flatCategoreies);
            }
            return flatCategoreies;
        }
        public async Task<IEnumerable<Category>> ListByItemAsync(ISpecification<Item> itemSpec)
        {
            var categories = await _itemRepository.ListRelatedAsync(itemSpec, (i => i.Category));
            List<Category> flatCategories = new List<Category>();
            foreach (var category in categories)
                await GetCategoryHierarchyAsync(category, flatCategories);
            return flatCategories;
        }
        public async Task GetCategoryHierarchyAsync(ISpecification<Category> spec, List<Category> hierarchy)
        {
            var category = await _repository.GetSingleBySpecAsync(spec);
            if (category != null)
                await GetCategoryHierarchyAsync(category, hierarchy);
        }
        public async Task GetCategoryHierarchyAsync(Category category, List<Category> hierarchy)
        {
            if (category != null)
            {
                if (!hierarchy.Contains(category))
                {
                    hierarchy.Add(category);
                    if (category.ParentCategoryId != 0)
                    {
                        await GetCategoryHierarchyAsync(new Specification<Category>(category.ParentCategoryId), hierarchy);
                    }
                }
            }
        }
        private async Task GetCategorySubcategoriesAsync(ISpecification<Category> spec, List<Category> subcategories)
        {
            var category = await _repository.GetSingleBySpecAsync(spec);
            if (category != null)
            {
                if (!subcategories.Contains(category))
                {
                    subcategories.Add(category);
                    foreach (var c in category.Subcategories)
                    {
                        var subcategorySpec = new Specification<Category>((s => s.Id == c.Id), (s => s.Subcategories));
                        subcategorySpec.Take = _maxTake;
                        await GetCategorySubcategoriesAsync(subcategorySpec, subcategories);
                    }
                }
            }
        }
    }
}
