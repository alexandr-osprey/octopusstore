import { Entity } from "../entity";
import { MeasurementUnit } from "./measurement-unit";
import { EntityDetail } from "../entity-detail";

export class MeasurementUnitDetail extends EntityDetail<MeasurementUnit> {

  public constructor(init?: Partial<MeasurementUnitDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
