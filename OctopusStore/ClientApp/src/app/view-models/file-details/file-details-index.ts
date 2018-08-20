import { Entity } from "../entity";
import { EntityIndex } from "../entity-index";

export abstract class FileDetailsIndex<T extends Entity> extends EntityIndex<T> {

  public constructor(init?: Partial<FileDetailsIndex<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
