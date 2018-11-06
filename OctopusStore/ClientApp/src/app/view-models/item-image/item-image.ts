import { Item } from "../item/item";
import { Image } from "../image/image";

export class ItemImage extends Image<Item>{

  public constructor(init?: Partial<ItemImage>) {
    super(init);
    Object.assign(this, init);
  }
}
