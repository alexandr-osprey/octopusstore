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
        public Category WomensClothing { get; protected set; }

        public Category Smartphones { get; protected set; }
        public Category Smartwatches { get; protected set; }
        
        public Category WomensFootwear { get; protected set; }
        public Category WomensDresses { get; protected set; }
        public Category WomensAccesories { get; protected set; }


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
                new Category { Title = "Womens clothing", CanHaveItems = false, OwnerId = Users.AdminId, }, //2

                new Category { Title = "Smartphones",  CanHaveItems = true, OwnerId = Users.AdminId, }, //3
                new Category { Title = "Smartwatches", CanHaveItems = true, OwnerId = Users.AdminId, }, //4

                new Category { Title = "Womens footwear", CanHaveItems = true, OwnerId = Users.AdminId, }, //5
                new Category { Title = "Dresses", CanHaveItems = true, OwnerId = Users.AdminId, }, //6
                new Category { Title = "Womens accesories", CanHaveItems = true, OwnerId = Users.AdminId, }, //7
            };
            return catetories;
        }

        protected override void AfterSeed(IEnumerable<Category> categories)
        {
            categories.ElementAt(1).ParentCategoryId = categories.ElementAt(0).Id;
            categories.ElementAt(2).ParentCategoryId = categories.ElementAt(0).Id;

            categories.ElementAt(3).ParentCategoryId = categories.ElementAt(1).Id;
            categories.ElementAt(4).ParentCategoryId = categories.ElementAt(1).Id;

            categories.ElementAt(5).ParentCategoryId = categories.ElementAt(2).Id;
            categories.ElementAt(6).ParentCategoryId = categories.ElementAt(2).Id;
            categories.ElementAt(7).ParentCategoryId = categories.ElementAt(2).Id;
            Context.SaveChanges();
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
            WomensClothing = Entities[2];

            Smartphones = Entities[3];
            Smartwatches = Entities[4];
            
            WomensFootwear = Entities[5];
            WomensDresses = Entities[6];
            WomensAccesories = Entities[7];
        }
    }
}
