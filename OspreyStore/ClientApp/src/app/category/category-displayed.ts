import { Category } from "./category";

export class CategoryDisplayed extends Category {
  expanded: boolean;
  currentSubcategory: Category;

  public constructor(init?: Partial<CategoryDisplayed>) {
    super(init);
    Object.assign(this, init);
    this.expanded = false;
  }
}
