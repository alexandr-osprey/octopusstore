import { Entity } from "./entity";
import { Index } from ".";

export class EntityIndex<T extends Entity> extends Index<T> {

  public constructor(init?: Partial<EntityIndex<T>>) {
    super(init);
  }
}
