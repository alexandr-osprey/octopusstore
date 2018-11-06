import { CartItem } from "./cart-item";
import { Entity } from "../entity/entity";

export class CartItemThumbnail extends Entity {

  public constructor(init?: Partial<CartItem>) {
    super(init);
    Object.assign(this, init);
  }
}
