import { Item } from './item';
import { ItemImage } from '../item-image/item-image';
import { Brand } from '../brand/brand';
import { ItemVariant } from '../item-variant/item-variant';

export class ItemThumbnail extends Item {
  itemVariants: ItemVariant[];
  prices: number[];
  minPrice: number;
  maxPrice: number;
  images: ItemImage[];
  brand: Brand;
  variantsTitles: string;

  public constructor(init?: Partial<ItemThumbnail>) {
    super(init);
    Object.assign(this, init);
    this.prices = init.itemVariants.map(v => v.price);
    this.variantsTitles = init.itemVariants.map(v => v.title).join(', ');
  }
}
