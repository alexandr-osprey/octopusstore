import { Item } from "./item";
import { Category } from "../category/category";
import { Brand } from "../brand/brand";
import { MeasurementUnit } from "../measurement-unit/measurement-unit";
import { Store } from "../store/store";
import { ItemVariant } from "../item-variant/item-variant";
import { ItemVariantImage } from "../item-variant-image/item-variant-image";


export class ItemDetail extends Item {
  measurementUnit: MeasurementUnit;
  category: Category;
  store: Store;
  brand: Brand;
  //images: ItemVariantImage[];
  itemVariants: ItemVariant[];

  public constructor(init?: Partial<ItemDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
