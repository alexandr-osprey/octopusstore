import { CartItem } from "./cart-item";
import { ItemVariant } from "src/app/item-variant/item-variant";
import { MeasurementUnit } from "src/app/measurement-unit/measurement-unit";
import { Item } from "src/app/item/item";
import { Store } from "src/app/store/store";
import { ItemImage } from "src/app/item-image/item-image";

export class CartItemThumbnail extends CartItem {
  itemVariant: ItemVariant;
  item: Item;
  //measurementUnit: MeasurementUnit;
  store: Store;
  itemImage: ItemImage;

  public constructor(init?: Partial<CartItemThumbnail>) {
    super(init);
    Object.assign(this, init);
    this.itemVariant = new ItemVariant(init.itemVariant);
    this.item = new Item(init.item);
    //this.measurementUnit = new MeasurementUnit(init.measurementUnit);
    this.itemImage = new ItemImage(init.itemImage);
  }
}
