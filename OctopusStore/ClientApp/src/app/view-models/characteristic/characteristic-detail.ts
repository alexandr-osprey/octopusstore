import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { Characteristic } from "./characteristic";

export class CharacteristicDetail extends EntityDetail<Characteristic> {

  public constructor(init?: Partial<CharacteristicDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
