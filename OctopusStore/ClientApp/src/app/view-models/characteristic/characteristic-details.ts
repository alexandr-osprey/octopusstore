import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { Characteristic } from "./characteristic";

export class CharacteristicDetails extends EntityDetail<Characteristic> {

  public constructor(init?: Partial<CharacteristicDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
