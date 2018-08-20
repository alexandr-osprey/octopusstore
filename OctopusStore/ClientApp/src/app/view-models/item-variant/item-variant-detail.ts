import { Entity } from "../entity";
import { ItemVariantCharacteristicValue } from "../item-variant-characteristic-value/item-variant-characteristic-value";
import { ItemVariant } from "./item-variant";
import { EntityDetail } from "../entity-detail";

export class ItemVariantDetail extends EntityDetail<ItemVariant> {
  itemId: number;
  price: number;
  itemVariantCharacteristicValues: ItemVariantCharacteristicValue[];

  public constructor(init?: Partial<ItemVariantDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
