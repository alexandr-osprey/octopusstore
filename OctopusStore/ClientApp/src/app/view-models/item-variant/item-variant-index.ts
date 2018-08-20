import { ItemVariant } from './item-variant';
import { EntityIndex } from '../entity-index';

export class ItemVariantIndex extends EntityIndex<ItemVariant> {

  public constructor(init?: Partial<ItemVariantIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
