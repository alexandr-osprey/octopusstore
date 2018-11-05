import { EntityIndex } from './entity-index';
import { ItemThumbnail } from './item-thumbnail';

export class ItemThumbnailIndex extends EntityIndex<ItemThumbnail> {

  public constructor(init?: Partial<ItemThumbnailIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
