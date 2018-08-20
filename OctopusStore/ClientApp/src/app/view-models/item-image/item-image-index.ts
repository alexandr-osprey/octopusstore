import { ImageIndex } from "../image/image-index";
import { ItemImage } from "./item-image";

export class ItemImageIndex extends ImageIndex<ItemImage> {

  public constructor(init?: Partial<ItemImageIndex>) {
    super(init);
    Object.assign(this, init);
  }
}
