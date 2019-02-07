import { Order } from "./order";
import { ItemVariant } from "../item-variant/item-variant";
import { Store } from "../store/store";

export class OrderThumbnail extends Order {
  itemVariant: ItemVariant;
  store: Store;

  public constructor(init?: Partial<OrderThumbnail>) {
    super(init);
    Object.assign(this, init);
  }
}
