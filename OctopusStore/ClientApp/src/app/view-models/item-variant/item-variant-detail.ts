import { ItemVariant } from "./item-variant";
import { EntityDetail } from "../entity-detail";
import { ItemProperty } from "../item-property/item-property";

export class ItemVariantDetail extends EntityDetail<ItemVariant> {
  itemId: number;
  price: number;
  itemProperties: ItemProperty[];

  public constructor(init?: Partial<ItemVariantDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
