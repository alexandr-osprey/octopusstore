import { Entity } from "../models/entity/entity";

export class ItemVariant extends Entity {
  title: string;
  itemId: number;
  price: number;

  public constructor(init?: Partial<ItemVariant>) {
    super(init);
    Object.assign(this, init);
  }
}
