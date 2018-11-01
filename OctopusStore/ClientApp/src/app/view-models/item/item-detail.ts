import { Entity } from '../entity';
import { Brand } from '../brand/brand';
import { ItemVariant } from '../item-variant/item-variant';
import { EntityDetail } from '../entity-detail';
import { Item } from './item';
import { ItemImage } from '../item-image/item-image';
import { Store } from '../store/store';
import { Category } from '../category/category';
import { MeasurementUnit } from '../measurement-unit/measurement-unit';

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
