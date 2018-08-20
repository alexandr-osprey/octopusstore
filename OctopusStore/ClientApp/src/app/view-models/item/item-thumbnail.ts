import { Entity } from '../entity';
import { ItemImage } from '../item-image/item-image';
import { Item } from './item';

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
