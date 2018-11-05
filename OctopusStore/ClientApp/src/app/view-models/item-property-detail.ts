import { Characteristic } from "./characteristic";
import { CharacteristicValue } from "./characteristic-value";
import { EntityDetail } from "./entity-detail";
import { ItemProperty } from "./item-property";

export class ItemPropertyDetail extends EntityDetail<ItemProperty> {
  itemVariantId: number;
  characteristic: Characteristic;
  characteristicValue: CharacteristicValue;

  public constructor(init?: Partial<ItemPropertyDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
