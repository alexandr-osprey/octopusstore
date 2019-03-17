import { Category } from "../category";

export class CategoryHierarchy extends Category {
  public allCategories: Category[];
  public parentCategories: Category[];
  constructor(private rootCategoryId: number, category: Category, allCategories: Category[]) {
    super(category);
    this.allCategories = allCategories;
    if (category) {
      this.parentCategories = [];
      this.setParentCategories(category, this.parentCategories);
      this.subcategories = this.allCategories.filter(c => c.parentCategoryId == category.id);
    }
  }

  setParentCategories(currentCategory: Category, parentCategories: Category[]) {
    if (currentCategory.id != this.rootCategoryId) 
      this.setParentCategories(this.allCategories.find(c => c.id == currentCategory.parentCategoryId), parentCategories);
    parentCategories.push(currentCategory);
  }
}
