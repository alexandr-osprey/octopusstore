import { Entity } from "../entity";
import { EntityIndex } from "../entity-index";
import { FileDetailsIndex } from "../file-details/file-details-index";

export abstract class ImageIndex<T extends Entity> extends FileDetailsIndex<T> {

  public constructor(init?: Partial<ImageIndex<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
