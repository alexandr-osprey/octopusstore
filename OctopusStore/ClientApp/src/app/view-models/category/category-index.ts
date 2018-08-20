import { Category } from "./category";
import { EntityIndex } from "../entity-index";

export class CategoryIndex extends EntityIndex<Category> {

  public constructor(init?: Partial<CategoryIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
