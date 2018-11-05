import { Entity } from "./entity";

export class Characteristic extends Entity {
  title: string;

  public constructor(init?: Partial<Characteristic>) {
    super(init);
    Object.assign(this, init);
  }
}
