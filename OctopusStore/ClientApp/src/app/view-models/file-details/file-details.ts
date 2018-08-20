import { Entity } from "../entity";

export abstract class FileDetails<T extends Entity> extends Entity {
  title: string;
  contentType: string;
  relatedId: number;
  ownerUsername: string;

  formFile: FormData;

  public constructor(init?: Partial<FileDetails<T>>) {
    super(init);
    Object.assign(this, init);
  }
}
