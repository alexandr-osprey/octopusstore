import { Category } from "./category";

export class CategoryDisplayed extends Category {
  collapsed: boolean;

  public constructor(init?: Partial<CategoryDisplayed>) {
    super(init);
    Object.assign(this, init);
    this.collapsed = true;
  }
}
