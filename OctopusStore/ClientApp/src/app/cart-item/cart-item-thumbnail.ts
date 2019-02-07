import { ItemVariant } from "../item-variant/item-variant";
import { Item } from "../item/item";
import { MeasurementUnit } from "../measurement-unit/measurement-unit";
import { Store } from "../store/store";
import { CartItem } from "./cart-item";

export class CartItemThumbnail extends CartItem {
  itemVariant: ItemVariant;
  item: Item;
  measurementUnit: MeasurementUnit;
  store: Store;

  public constructor(init?: Partial<CartItemThumbnail>) {
    super(init);
    Object.assign(this, init);
    this.itemVariant = new ItemVariant(init.itemVariant);
    this.item = new Item(init.item);
    this.measurementUnit = new MeasurementUnit(init.measurementUnit);
  }
}
