import { Entity } from "../entity/entity";

export class Characteristic extends Entity {
  title: string;
  categoryId: number;

  public constructor(init?: Partial<Characteristic>) {
    super(init);
    Object.assign(this, init);
  }
}
