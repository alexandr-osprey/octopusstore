import { Item } from './item';
import { ItemImage } from '../item-image/item-image';

export class ItemThumbnail extends Item {
  prices: number[];
  minPrice: number;
  maxPrice: number;
  images: ItemImage[];

  public constructor(init?: Partial<ItemThumbnail>) {
    super(init);
    Object.assign(this, init);
  }
}
