import { Entity } from "../entity";
import { EntityIndex } from "../entity-index";
import { FileInfoIndex } from "../file-info/file-info-index";

export abstract class ImageIndex<T extends Entity> extends FileInfoIndex<T> {

  public constructor(init?: Partial<ImageIndex<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
