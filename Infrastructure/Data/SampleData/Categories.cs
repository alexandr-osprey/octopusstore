using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class Categories: SampleDataEntities<Category>
    {
        public Category Root { get; protected set; }
        public Category Electronics { get; protected set; }
        public Category Smartphones { get; protected set; }
        public Category Smartwatches { get; protected set; }
        public Category Clothes { get; protected set; }
        public Category Shoes { get; protected set; }
        public Category Jackets { get; protected set; }

        public Categories(StoreContext storeContext): base(storeContext)
        {
            Seed();
            Init();
        }

        protected override IEnumerable<Category> GetSourceEntities()
        {
            var catetories = new List<Category>
            {
                new Category { Title = "Categories", CanHaveItems = false, OwnerId = Users.AdminId, IsRoot = true },
                new Category { Title = "Electronics",  CanHaveItems = false, OwnerId = Users.AdminId }, //1
                new Category { Title = "Smartphones",  CanHaveItems = true, OwnerId = Users.AdminId}, //2
                new Category { Title = "Smartwatches", CanHaveItems = true, OwnerId = Users.AdminId}, //3

                new Category { Title = "Clothes", CanHaveItems = false, OwnerId = Users.AdminId}, //4
                new Category { Title = "Shoes", CanHaveItems = true, OwnerId = Users.AdminId}, //5
                new Category { Title = "Jackets", CanHaveItems = true, OwnerId = Users.AdminId}, //6
            };
            return catetories;
        }

        protected override void AfterSeed(IEnumerable<Category> categories)
        {
            categories.ElementAt(1).ParentCategoryId = categories.ElementAt(0).Id;
            categories.ElementAt(4).ParentCategoryId = categories.ElementAt(0).Id;
            categories.ElementAt(2).ParentCategoryId = categories.ElementAt(1).Id;
            categories.ElementAt(3).ParentCategoryId = categories.ElementAt(1).Id;
            categories.ElementAt(5).ParentCategoryId = categories.ElementAt(4).Id;
            categories.ElementAt(6).ParentCategoryId = categories.ElementAt(4).Id;
            StoreContext.SaveChanges();
            base.AfterSeed(categories);
        }

        protected override IQueryable<Category> GetQueryable()
        {
            return base.GetQueryable().Include(c => c.Items).Include(c => c.Subcategories);
        }

        public override void Init()
        {
            base.Init();
            Root = Entities[0];
            Electronics = Entities[1];
            Smartphones = Entities[2];
            Smartwatches = Entities[3];
            Clothes = Entities[4];
            Shoes = Entities[5];
            Jackets = Entities[6];
        }
    }
}
