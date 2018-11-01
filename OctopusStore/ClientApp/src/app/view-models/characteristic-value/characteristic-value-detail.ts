import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { CharacteristicValue } from "./characteristic-value";

export class CharacteristicValueDetail extends EntityDetail<CharacteristicValue> {
  categoryPropertyId: number;

  public constructor(init?: Partial<CharacteristicValueDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
