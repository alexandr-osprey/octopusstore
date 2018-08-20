import { Entity } from "../entity";
import { Characteristic } from "../characteristic/characteristic";
import { CharacteristicValue } from "../characteristic-value/characteristic-value";
import { ItemVariantCharacteristicValue } from "./item-variant-characteristic-value";
import { EntityDetail } from "../entity-detail";

export class ItemVariantCharacteristicValueDetails extends EntityDetail<ItemVariantCharacteristicValue> {
  itemVariantId: number;
  characteristic: Characteristic;
  characteristicValue: CharacteristicValue;

  public constructor(init?: Partial<ItemVariantCharacteristicValueDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
