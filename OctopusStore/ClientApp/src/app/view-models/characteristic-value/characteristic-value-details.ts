import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { CharacteristicValue } from "./characteristic-value";

export class CharacteristicValueDetails extends EntityDetail<CharacteristicValue> {
  categoryPropertyId: number;

  public constructor(init?: Partial<CharacteristicValueDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
