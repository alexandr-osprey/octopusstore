import { Entity } from "../models/entity/entity";

export class Item extends Entity {
  title: string;
  categoryId: number;
  storeId: number;
  brandId: number;
  description: string;

  public constructor(init?: Partial<Item>) {
    super(init);
    Object.assign(this, init);
  }
}
