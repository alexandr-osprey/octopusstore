using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CategoryService: Service<Category>, ICategoryService
    {
        public int RootCategoryId { get; set; } = 1;

        public CategoryService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Category> authoriationParameters,
            IAppLogger<Service<Category>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }

        public async Task<IEnumerable<Category>> EnumerateHierarchyAsync(Specification<Item> spec)
        {
            var categories = await _context.EnumerateRelatedAsync(_logger, spec, (i => i.Category));
            return await EnumerateHierarchyAsync(new EntitySpecification<Category>(c => categories.Contains(c)));
        }
        public async Task<IEnumerable<Category>> EnumerateHierarchyAsync(Specification<Category> spec)
        {
            var hierarchy = new HashSet<Category>();
            hierarchy.UnionWith(await EnumerateParentCategoriesAsync(spec));
            hierarchy.UnionWith(await EnumerateSubcategoriesAsync(spec));
            return hierarchy;
        }
        public async Task<IEnumerable<Category>> EnumerateParentCategoriesAsync(Specification<Category> spec)
        {
            var flatCategories = new HashSet<Category>();
            await GetParentCategoriesAsync(spec, flatCategories);
            return flatCategories;
        }
        public async Task<IEnumerable<Category>> EnumerateSubcategoriesAsync(Specification<Category> spec)
        {
            var flatCategories = new HashSet<Category>();
            var categories = await _context.EnumerateAsync(_logger, spec);
            foreach (var category in categories)
            {
                var categorySpec = new Specification<Category>((s => s.Id == category.Id), (s => s.Subcategories))
                {
                    Description = $"Category id={category.Id} includes Subcategories"
                };
                await GetSubcategoriesAsync(categorySpec, flatCategories);
            }
            return flatCategories;
        }
        public async Task<IEnumerable<Category>> EnumerateParentCategoriesAsync(Specification<Item> itemSpec)
        {
            var categories = (await _context.EnumerateRelatedAsync(_logger, itemSpec, (i => i.Category))).Distinct();
            var flatCategories = new HashSet<Category>();
            foreach (var category in categories)
                await GetParentCategoriesAsync(category, flatCategories);
            return flatCategories;
        }
        public async Task<IEnumerable<Category>> EnumerateSubcategoriesAsync(Specification<Item> itemSpec)
        {
            var categories = await _context.EnumerateRelatedAsync(_logger, itemSpec, (i => i.Category));
            var flatCategories = new HashSet<Category>();
            foreach (var category in categories)
                await GetSubcategoriesAsync(category, flatCategories);
            return flatCategories;
        }
        public async Task GetParentCategoriesAsync(Specification<Category> spec, HashSet<Category> hierarchy)
        {
            var categories = await _context.EnumerateAsync(_logger, spec);
            foreach (var c in categories)
                await GetParentCategoriesAsync(c, hierarchy);
        }
        public async Task GetParentCategoriesAsync(Category category, HashSet<Category> hierarchy)
        {
            if (category != null)
            {
                if (!hierarchy.Contains(category))
                {
                    hierarchy.Add(category);
                    if (category.ParentCategoryId != 0)
                    {
                        await GetParentCategoriesAsync(new Specification<Category>(c => c.Id == category.ParentCategoryId), hierarchy);
                    }
                }
            }
        }

        protected override async Task ValidateCreateWithExceptionAsync(Category category)
        {
            await base.ValidateCreateWithExceptionAsync(category);
            if (string.IsNullOrWhiteSpace(category.Title))
                throw new EntityValidationException("Title not specified");
            if (string.IsNullOrWhiteSpace(category.Description))
                throw new EntityValidationException("Description not specified");
            if (!await _context.ExistsBySpecAsync(_logger, new EntitySpecification<Category>(category.ParentCategoryId)))
                throw new EntityValidationException("Parent category does not exist");
        }
        protected override async Task ValidateUpdateWithExceptionAsync(Category category)
        {
            await ValidateCreateWithExceptionAsync(category);
        }
        protected async Task GetSubcategoriesAsync(Category category, HashSet<Category> subcategories)
        {
            if (category != null)
            {
                subcategories.Add(category);
                foreach (var c in category.Subcategories)
                {
                    var subcategorySpec = new Specification<Category>(s => s.Id == c.Id, s => s.Subcategories)
                    {
                        Description = $"Category Id = {c.Id} includes Subcategories"
                    };
                    await GetSubcategoriesAsync(subcategorySpec, subcategories);
                }
            }
        }
        protected async Task GetSubcategoriesAsync(Specification<Category> spec, HashSet<Category> subcategories)
        {
            var category = await _context.ReadSingleBySpecAsync(_logger, spec, false);
            await GetSubcategoriesAsync(category, subcategories);
        }
    }
}
