export abstract class Entity {
  id: number;

  public constructor(init?: Partial<Entity>) {
    this.id = 0;
    Object.assign(this, init);
  }
}
