import { ItemProperty } from "../item-property/item-property";
import { ItemVariantThumbnail } from "./item-variant-thumbnail";

export class ItemVariantDetail extends ItemVariantThumbnail {
  itemProperties: ItemProperty[];

  public constructor(init?: Partial<ItemVariantDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
