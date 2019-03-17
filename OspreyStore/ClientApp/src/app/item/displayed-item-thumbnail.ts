import { ItemThumbnail } from './item-thumbnail';

export class DisplayedItemThumbnail extends ItemThumbnail {
  page: number;

  public constructor(init?: Partial<DisplayedItemThumbnail>) {
    super(init);
    Object.assign(this, init);
  }
}
