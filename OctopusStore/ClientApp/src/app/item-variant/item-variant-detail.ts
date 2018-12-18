import { ItemVariant } from "./item-variant";
import { ItemProperty } from "../item-property/item-property";
import { EntityDetail } from "../models/entity/entity-detail";

export class ItemVariantDetail extends EntityDetail<ItemVariant> {
  itemId: number;
  price: number;
  itemProperties: ItemProperty[];

  public constructor(init?: Partial<ItemVariantDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
