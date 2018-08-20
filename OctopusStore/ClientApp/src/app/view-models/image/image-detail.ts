import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { FileInfoDetails } from "../file-info/file-info-details";

export abstract class ImageDetails<T extends Entity> extends FileInfoDetails<T> {

  public constructor(init?: Partial<ImageDetails<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
