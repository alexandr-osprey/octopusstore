import { EntityIndex } from "../entity-index";
import { CharacteristicValue } from "./characteristic-value";

export class CharacteristicValueIndex extends EntityIndex<CharacteristicValue> {

  public constructor(init?: Partial<CharacteristicValueIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
