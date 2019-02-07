import { Entity } from "../models/entity/entity";

export enum OrderStatus {
  Created,
  Finished,
  Cancelled
}

export class Order extends Entity {
  storeId: number;
  dateTimeCreated: Date;
  dateTimeFinished: Date;
  dateTimeCancelled: Date;
  status: OrderStatus;
  sum: number;
  itemVariantId: number;
  number: number;

  public constructor(init?: Partial<Order>) {
    super(init);
    Object.assign(this, init);
  }
}
