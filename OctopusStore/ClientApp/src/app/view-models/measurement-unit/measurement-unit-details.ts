import { Entity } from "../entity";
import { MeasurementUnit } from "./measurement-unit";
import { EntityDetail } from "../entity-detail";

export class MeasurementUnitDetails extends EntityDetail<MeasurementUnit> {

  public constructor(init?: Partial<MeasurementUnitDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
