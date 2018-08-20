import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { Category } from "./category";

export class CategoryDetails extends EntityDetail<Category> {
  parentCategoryId: number;
  description: string;
  subcategories: Category[];

  public constructor(init?: Partial<Category>) {
    super(init);
    Object.assign(this, init);
  }
}
