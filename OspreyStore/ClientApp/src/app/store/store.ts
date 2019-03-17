import { Entity } from "../models/entity/entity";

export class Store extends Entity {
  title: string;
  ownerId: string;
  description: string;
  address: string;
  registrationDate: string;

  public constructor(init?: Partial<Store>) {
    super(init);
    Object.assign(this, init);
  }
}
