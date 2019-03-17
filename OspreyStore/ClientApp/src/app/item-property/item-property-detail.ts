import { Characteristic } from "../characteristic/characteristic";
import { CharacteristicValue } from "../characteristic-value/characteristic-value";
import { ItemProperty } from "./item-property";
import { EntityDetail } from "../models/entity/entity-detail";

export class ItemPropertyDetail extends EntityDetail<ItemProperty> {
  itemVariantId: number;
  characteristic: Characteristic;
  characteristicValue: CharacteristicValue;

  public constructor(init?: Partial<ItemPropertyDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
