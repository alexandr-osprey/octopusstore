import { Characteristic } from "../characteristic/characteristic";
import { CharacteristicValue } from "../characteristic-value/characteristic-value";
import { EntityDetail } from "../entity-detail";
import { ItemProperty } from "./item-property";

export class ItemPropertyDetails extends EntityDetail<ItemProperty> {
  itemVariantId: number;
  characteristic: Characteristic;
  characteristicValue: CharacteristicValue;

  public constructor(init?: Partial<ItemPropertyDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
