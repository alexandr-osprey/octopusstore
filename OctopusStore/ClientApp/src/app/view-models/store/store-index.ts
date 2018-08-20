import { Store } from "./store";
import { EntityIndex } from "../entity-index";

export class StoreIndex extends EntityIndex<Store> {

  public constructor(init?: Partial<StoreIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
