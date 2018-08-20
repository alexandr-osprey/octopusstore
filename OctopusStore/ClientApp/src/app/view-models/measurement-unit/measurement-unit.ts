import { Entity } from "../entity";

export class MeasurementUnit extends Entity {
  title: string;

  public constructor(init?: Partial<MeasurementUnit>) {
    super(init);
    Object.assign(this, init);
  }
}
