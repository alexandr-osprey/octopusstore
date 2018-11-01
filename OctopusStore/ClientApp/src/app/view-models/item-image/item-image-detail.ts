import { Item } from "../item/item";
import { ImageDetail } from "../image/image-detail";

export class ItemImageDetail extends ImageDetail<Item> {

  public constructor(init?: Partial<ItemImageDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
