using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.SampleData
{
    public class Categories: SampleDataEntities<Category>
    {
        public Category Root { get; }
        public Category Electronics { get; }
        public Category Smartphones { get; }
        public Category Smartwatches { get; }
        public Category Clothes { get; }
        public Category Shoes { get; }
        public Category Jackets { get; }

        public Categories(StoreContext storeContext): base(storeContext)
        {
            Seed();
            Root = Entities[0];
            Electronics = Entities[1];
            Smartphones = Entities[2];
            Smartwatches = Entities[3];
            Clothes = Entities[4];
            Shoes = Entities[5];
            Jackets = Entities[6];
        }

        protected override IEnumerable<Category> GetSourceEntities()
        {
            return new List<Category>
            {
                new Category { Title = "Categories", CanHaveItems = true, OwnerId = Users.AdminId },
                new Category { Title = "Electronics",  ParentCategoryId = 1, CanHaveItems = false, OwnerId = Users.AdminId }, //1
                new Category { Title = "Smartphones",  ParentCategoryId = 2, CanHaveItems = true, OwnerId = Users.AdminId}, //2
                new Category { Title = "Smartwatches", ParentCategoryId = 2, CanHaveItems = true, OwnerId = Users.AdminId}, //3

                new Category { Title = "Clothes",  ParentCategoryId = 1, CanHaveItems = false, OwnerId = Users.AdminId}, //4
                new Category { Title = "Shoes", ParentCategoryId = 5, CanHaveItems = true, OwnerId = Users.AdminId}, //5
                new Category { Title = "Jackets", ParentCategoryId = 5, CanHaveItems = true, OwnerId = Users.AdminId}, //6
            };
        }
    }
}
