import { Item } from "./item";
import { Image } from "./image";

export class ItemImage extends Image<Item>{

  public constructor(init?: Partial<ItemImage>) {
    super(init);
    Object.assign(this, init);
  }
}
