import { EntityIndex } from "../entity-index";
import { Characteristic } from "./characteristic";

export class CharacteristicIndex extends EntityIndex<Characteristic> {

  public constructor(init?: Partial<CharacteristicIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
