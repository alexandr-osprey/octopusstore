import { Entity } from "./entity";

export abstract class EntityDetail<T extends Entity> {
  id: number;
  title: string;

  public constructor(init?: Partial<EntityDetail<T>>) {
    Object.assign(this, init);
  }
}
