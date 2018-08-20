import { Category } from "../view-models/category/category";

export class CategoryDisplayed extends Category {
  public static allCategories: Category[];
  public static rootCategoryId: number = 1;

  public parentCategories: Category[];
  constructor(init?: Partial<Category>) {
    super(init);
    if (init) {
      let parentCategory = CategoryDisplayed.allCategories.find(c => c.id == init.parentCategoryId);
      this.parentCategories = [];
      if (parentCategory)
        this.setParentCategories(parentCategory, this.parentCategories);
      this.subcategories = CategoryDisplayed.allCategories.filter(c => c.parentCategoryId == init.id);
    }
  }

  setParentCategories(currentCategory: Category, parentCategories: Category[]) {
    if (currentCategory.id != CategoryDisplayed.rootCategoryId) 
      this.setParentCategories(CategoryDisplayed.allCategories.find(c => c.id == currentCategory.parentCategoryId), parentCategories);
    parentCategories.push(currentCategory);
  }
}
