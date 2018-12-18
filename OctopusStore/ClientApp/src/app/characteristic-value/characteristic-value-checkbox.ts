import { CharacteristicValue } from "./characteristic-value";

export class CharacteristicValueCheckbox extends CharacteristicValue {
  checked: boolean;

  public constructor(init?: Partial<CharacteristicValue>) {
    super(init);
    Object.assign(this, init);
  }
}
