import { Entity } from "./entity";

export class Category extends Entity {
  title: string;
  parentCategoryId: number;
  description: string;
  subcategories: Category[];

  public constructor(init?: Partial<Category>) {
    super(init);
    Object.assign(this, init);
  }
}
