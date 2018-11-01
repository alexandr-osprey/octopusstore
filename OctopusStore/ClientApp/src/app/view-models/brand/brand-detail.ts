import { Entity } from "../entity";
import { Brand } from "./brand";
import { EntityDetail } from "../entity-detail";

export class BrandDetail extends EntityDetail<Brand> {

  public constructor(init?: Partial<BrandDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
