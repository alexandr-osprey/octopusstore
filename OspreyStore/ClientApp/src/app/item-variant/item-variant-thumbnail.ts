import { Entity } from "../models/entity/entity";
import { ItemVariantImage } from "../item-variant-image/item-variant-image";
import { ItemVariant } from "./item-variant";

export class ItemVariantThumbnail extends ItemVariant {
  images: ItemVariantImage[];

  public constructor(init?: Partial<ItemVariantThumbnail>) {
    super(init);
    Object.assign(this, init);
  }
}
