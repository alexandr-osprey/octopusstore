import { EntityIndex } from "../entity-index";
import { ItemVariantCharacteristicValue } from "./item-variant-characteristic-value";

export class ItemVariantCharacteristicValueIndex extends EntityIndex<ItemVariantCharacteristicValue> {

  public constructor(init?: Partial<ItemVariantCharacteristicValueIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
