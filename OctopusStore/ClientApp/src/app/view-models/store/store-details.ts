import { Entity } from "../entity";
import { Store } from "./store";
import { EntityDetail } from "../entity-detail";

export class StoreDetails extends EntityDetail<Store> {
  title: string;
  OwnerId: string;
  description: string;
  address: string;
  registrationDate: string;

  public constructor(init?: Partial<StoreDetails>) {
    super(init);
    Object.assign(this, init);
  }
}
