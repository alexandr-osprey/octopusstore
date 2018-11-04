import { Entity } from "../entity";
import { Store } from "./store";
import { EntityDetail } from "../entity-detail";

export class StoreDetail extends EntityDetail<Store> {
  title: string;
  ownerId: string;
  description: string;
  address: string;
  registrationDate: string;

  public constructor(init?: Partial<StoreDetail>) {
    super(init);
    Object.assign(this, init);
  }
}
