using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
            var catetories = new List<Category>
            {
                new Category { Title = "Categories", CanHaveItems = true, OwnerId = Users.AdminId },
                new Category { Title = "Electronics",  CanHaveItems = false, OwnerId = Users.AdminId }, //1
                new Category { Title = "Smartphones",  CanHaveItems = true, OwnerId = Users.AdminId}, //2
                new Category { Title = "Smartwatches", CanHaveItems = true, OwnerId = Users.AdminId}, //3

                new Category { Title = "Clothes", CanHaveItems = false, OwnerId = Users.AdminId}, //4
                new Category { Title = "Shoes", CanHaveItems = true, OwnerId = Users.AdminId}, //5
                new Category { Title = "Jackets", CanHaveItems = true, OwnerId = Users.AdminId}, //6
            };
            return catetories;
        }

        protected override void AfterSeed(List<Category> categories)
        {
            categories[1].ParentCategoryId = categories[0].Id;
            categories[4].ParentCategoryId = categories[0].Id;
            categories[2].ParentCategoryId = categories[1].Id;
            categories[3].ParentCategoryId = categories[1].Id;
            categories[5].ParentCategoryId = categories[4].Id;
            categories[6].ParentCategoryId = categories[4].Id;
            //_storeContext.UpdateRange(categories);
            StoreContext.SaveChanges();
            base.AfterSeed(categories);
        }
    }
}
