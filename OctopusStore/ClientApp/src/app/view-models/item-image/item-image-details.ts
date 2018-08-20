import { Item } from "../item/item";
import { ImageDetails } from "../image/image-detail";

export class ItemImageDetails extends ImageDetails<Item> {

  public constructor(init?: Partial<ItemImageDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
