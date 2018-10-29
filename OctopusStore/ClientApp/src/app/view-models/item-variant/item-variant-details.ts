import { ItemVariant } from "./item-variant";
import { EntityDetail } from "../entity-detail";
import { ItemProperty } from "../item-property/item-property";

export class ItemVariantDetails extends EntityDetail<ItemVariant> {
  itemId: number;
  price: number;
  itemProperties: ItemProperty[];

  public constructor(init?: Partial<ItemVariantDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
