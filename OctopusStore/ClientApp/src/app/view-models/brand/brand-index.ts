import { Brand } from "./brand";
import { EntityIndex } from "../entity-index";

export class BrandIndex extends EntityIndex<Brand> {

  public constructor(init?: Partial<BrandIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
