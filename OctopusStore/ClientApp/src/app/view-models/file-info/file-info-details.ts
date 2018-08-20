import { Entity } from "../entity";
import { EntityDetail } from "../entity-detail";

export abstract class FileInfoDetails<T extends Entity> extends EntityDetail<T> {
  contentType: string;
  relatedId: number;
  ownerUsername: string;

  public constructor(init?: Partial<FileInfoDetails<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
