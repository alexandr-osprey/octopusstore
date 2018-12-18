import { Item } from "./item";
import { Category } from "../category/category";
import { Brand } from "../brand/brand";
import { MeasurementUnit } from "../measurement-unit/measurement-unit";
import { Store } from "../store/store";
import { ItemImage } from "../item-image/item-image";
import { ItemVariant } from "../item-variant/item-variant";
import { EntityDetail } from "../models/entity/entity-detail";


export class ItemDetail extends EntityDetail<Item> {
  description: string;
  measurementUnit: MeasurementUnit;
  category: Category;
  store: Store;
  brand: Brand;
  images: ItemImage[];
  itemVariants: ItemVariant[];

  public constructor(init?: Partial<ItemDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
