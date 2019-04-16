using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Category for grouping Items. May be build into hierarchy.
    /// </summary>
    public class Category: Entity, ShallowClonable<Category>
    {
        public string Title { get; set; }
        public int ParentCategoryId { get; set; }
        public bool CanHaveItems { get; set; }
        public bool IsRoot { get; set; } = false;

        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
        public virtual ICollection<Category> Subcategories { get; set; } = new List<Category>();
        public virtual IEnumerable<Item> Items { get; set; } = new List<Item>();

        public Category(): base()
        {
        }

        public bool Equals(Category other) => base.Equals(other) 
            && Title.EqualsCI(other.Title) 
            && ParentCategoryId == other.ParentCategoryId 
            && CanHaveItems == other.CanHaveItems 
            && IsRoot == other.IsRoot;
        public override bool Equals(object obj) => Equals(obj as Category);
        public override int GetHashCode() => base.GetHashCode();

        protected Category(Category category): base(category)
        {
            Title = category.Title;
            ParentCategoryId = category.ParentCategoryId;
            CanHaveItems = category.CanHaveItems;
        }

        public Category ShallowClone()
        {
            return (Category)MemberwiseClone();
        }
    }
}
