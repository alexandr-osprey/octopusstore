import { Item } from '../item/item'
import { EntityIndex } from '../entity-index';

export class ItemIndex extends EntityIndex<Item> {

  public constructor(init?: Partial<ItemIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
