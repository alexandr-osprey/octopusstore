import { Entity } from "./entity";

export abstract class EntityIndex<T extends Entity> {
  page: number;
  totalPages: number;
  totalCount: number;
  entities: T[];

  public constructor(init?: Partial<EntityIndex<T>>) {
    Object.assign(this, init);
  }
}
