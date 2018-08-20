import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { FileDetailsDetails } from "../file-details/file-details-details";

export abstract class ImageDetail<T extends Entity> extends FileDetailsDetails<T> {

  public constructor(init?: Partial<ImageDetail<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
