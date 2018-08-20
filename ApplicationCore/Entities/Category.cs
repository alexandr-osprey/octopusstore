using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Category : Entity
    {
        public string Title { get; set; }
        public int ParentCategoryId { get; set; }
        public bool CanHaveItems { get; set; }
        public string Description { get; set; }

        public Category ParentCategory { get; set; }
        public ICollection<Characteristic> CategoryProperties { get; set; } = new List<Characteristic>();
        public ICollection<Category> Subcategories { get; set; } = new List<Category>();
    }
}
