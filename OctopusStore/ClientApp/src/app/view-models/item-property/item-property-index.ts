import { EntityIndex } from "../entity-index";
import { ItemProperty } from "./item-property";

export class ItemPropertyIndex extends EntityIndex<ItemProperty> {

  public constructor(init?: Partial<ItemPropertyIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
