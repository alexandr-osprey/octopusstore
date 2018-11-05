using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Category for grouping Items. May be build into hierarchy.
    /// </summary>
    public class Category: Entity, IGenericMemberwiseClonable<Category>
    {
        public string Title { get; set; }
        public int ParentCategoryId { get; set; }
        public string Description { get; set; }
        public bool CanHaveItems { get; set; }
        public bool IsRoot { get; set; } = false;

        public Category ParentCategory { get; set; }
        public ICollection<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
        public ICollection<Category> Subcategories { get; set; } = new List<Category>();
        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public Category(): base()
        {
        }

        public bool Equals(Category other) => base.Equals(other) 
            && Title.EqualsCI(other.Title) 
            && ParentCategoryId == other.ParentCategoryId 
            && Description.EqualsCI(other.Description)
            && CanHaveItems == other.CanHaveItems 
            && IsRoot == other.IsRoot;
        public override bool Equals(object obj) => Equals(obj as Category);
        public override int GetHashCode() => base.GetHashCode();

        protected Category(Category category): base(category)
        {
            Title = category.Title;
            ParentCategoryId = category.ParentCategoryId;
            Description = category.Description;
            CanHaveItems = category.CanHaveItems;
        }

        public Category ShallowClone()
        {
            return (Category)MemberwiseClone();
        }
    }
}
