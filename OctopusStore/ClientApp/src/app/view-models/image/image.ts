import { Entity } from "../entity";
import { FileDetails } from "../file-details/file-details";

export abstract class Image<T extends Entity> extends FileDetails<T> {

  public constructor(init?: Partial<Image<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
