import { Item } from './item';
import { Brand } from '../brand/brand';
import { ItemVariant } from '../item-variant/item-variant';
import { ItemVariantImage } from '../item-variant-image/item-variant-image';

export class ItemThumbnail extends Item {
  itemVariants: ItemVariant[];
  prices: number[];
  minPrice: number;
  maxPrice: number;
  images: ItemVariantImage[];
  brand: Brand;
  variantsTitles: string;

  public constructor(init?: Partial<ItemThumbnail>) {
    super(init);
    Object.assign(this, init);
    this.prices = init.itemVariants.map(v => v.price);
    this.variantsTitles = init.itemVariants.map(v => v.title).join(', ');
  }
}
