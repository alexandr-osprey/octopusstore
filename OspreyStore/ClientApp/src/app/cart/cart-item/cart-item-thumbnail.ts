import { CartItem } from "./cart-item";
import { ItemVariant } from "src/app/item-variant/item-variant";
import { Item } from "src/app/item/item";
import { Store } from "src/app/store/store";
import { ItemVariantImage } from "src/app/item-variant-image/item-variant-image";

export class CartItemThumbnail extends CartItem {
  itemVariant: ItemVariant;
  item: Item;
  //measurementUnit: MeasurementUnit;
  store: Store;
  itemVariantImage: ItemVariantImage;

  public constructor(init?: Partial<CartItemThumbnail>) {
    super(init);
    Object.assign(this, init);
    this.itemVariant = new ItemVariant(init.itemVariant);
    this.item = new Item(init.item);
    //this.measurementUnit = new MeasurementUnit(init.measurementUnit);
    this.itemVariantImage = new ItemVariantImage(init.itemVariantImage);
  }
}
