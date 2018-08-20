import { MeasurementUnitDetails } from "./measurement-unit-details";
import { MeasurementUnit } from "./measurement-unit";
import { EntityIndex } from "../entity-index";

export class MeasurementUnitIndex extends EntityIndex<MeasurementUnit> {

  public constructor(init?: Partial<MeasurementUnitIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
