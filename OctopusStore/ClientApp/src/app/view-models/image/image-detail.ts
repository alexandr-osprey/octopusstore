import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";
import { FileInfoDetail } from "../file-info/file-info-detail";

export abstract class ImageDetail<T extends Entity> extends FileInfoDetail<T> {

  public constructor(init?: Partial<ImageDetail<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
