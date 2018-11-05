import { EntityDetail } from "./entity-detail";
import { Item } from "./item";
import { MeasurementUnit } from "./measurement-unit";
import { Category } from "./category";
import { Store } from "./store";
import { Brand } from "./brand";
import { ItemImage } from "./item-image";
import { ItemVariant } from "./item-variant";

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
