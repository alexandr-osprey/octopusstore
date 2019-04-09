import { Image } from "./image";
import { ItemVariant } from "../item-variant/item-variant";

export class ItemVariantImage extends Image<ItemVariant>{
  shown: boolean;

  public constructor(init?: Partial<ItemVariantImage>) {
    super(init);
    Object.assign(this, init);
  }
}
