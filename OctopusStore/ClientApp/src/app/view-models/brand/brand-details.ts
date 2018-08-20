import { Entity } from "../entity";
import { Brand } from "./brand";
import { EntityDetail } from "../entity-detail";

export class BrandDetails extends EntityDetail<Brand> {

  public constructor(init?: Partial<BrandDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
