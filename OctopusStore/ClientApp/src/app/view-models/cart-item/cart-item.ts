import { Entity } from "../entity/entity";

export class CartItem extends Entity {
  itemVariantId: number;
  number: number;

  public constructor(init?: Partial<CartItem>) {
    super(init);
    Object.assign(this, init);
  }
}
