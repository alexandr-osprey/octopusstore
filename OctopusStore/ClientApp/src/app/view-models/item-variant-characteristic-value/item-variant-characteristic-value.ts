import { Entity } from "../entity";

export class ItemVariantCharacteristicValue extends Entity {
  itemVariantId: number;
  characteristicValueId: number;

  public constructor(init?: Partial<ItemVariantCharacteristicValue>) {
    super(init);
    Object.assign(this, init);
  }
}
