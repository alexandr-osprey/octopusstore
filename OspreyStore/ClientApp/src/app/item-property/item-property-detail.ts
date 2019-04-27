import { Characteristic } from "../characteristic/characteristic";
import { CharacteristicValue } from "../characteristic-value/characteristic-value";
import { ItemProperty } from "./item-property";
import { Entity } from "../models/entity/entity";

export class ItemPropertyDetail extends Entity {
  title: string;
  itemVariantId: number;
  characteristic: Characteristic;
  characteristicValue: CharacteristicValue;

  public constructor(init?: Partial<ItemPropertyDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
