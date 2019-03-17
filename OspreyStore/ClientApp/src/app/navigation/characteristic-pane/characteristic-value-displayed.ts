import { CharacteristicValue } from "src/app/characteristic-value/characteristic-value";

export class CharacteristicValueDisplayed extends CharacteristicValue {
  selected: boolean;

  public constructor(init?: Partial<CharacteristicValue>) {
    super(init);
    Object.assign(this, init);
  }
}
