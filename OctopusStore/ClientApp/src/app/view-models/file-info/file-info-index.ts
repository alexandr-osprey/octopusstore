import { Entity } from "../entity";
import { EntityIndex } from "../entity-index";

export abstract class FileInfoIndex<T extends Entity> extends EntityIndex<T> {

  public constructor(init?: Partial<FileInfoIndex<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
