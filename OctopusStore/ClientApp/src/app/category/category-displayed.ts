import { Category } from "./category";

export class CategoryDisplayed extends Category {
  collapsed: boolean;
  currentSubcategory: Category;

  public constructor(init?: Partial<CategoryDisplayed>) {
    super(init);
    Object.assign(this, init);
    this.collapsed = true;
  }
}
