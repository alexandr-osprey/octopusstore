import { Entity } from "../entity";
import { FileInfo } from "../file-info/file-info";

export abstract class Image<T extends Entity> extends FileInfo<T> {

  public constructor(init?: Partial<Image<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
