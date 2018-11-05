import { Characteristic } from "../../view-models/characteristic";
import { CharacteristicValue } from "../../view-models/characteristic-value";
import { ItemVariant } from "../../view-models/item-variant";
import { ItemProperty } from "../../view-models/item-property";

export class CharacteristicValueDisplayed extends ItemProperty {
  public characteristics: Characteristic[];
  public characteristicValues: CharacteristicValue[];
  private _characteristicId: number;

  public set characteristicId(characteristicId: number) {
    this._characteristicId = characteristicId;
    this.characteristic = this.characteristics.find(c => c.id == characteristicId);
  }
  public get characteristicId(): number {
    return this._characteristicId;
  }
  public characteristicValue: CharacteristicValue;
  public characteristic: Characteristic;
  public itemVariant: ItemVariant;

  constructor(characteristics: Characteristic[], characteristicValues: CharacteristicValue[], init?: Partial<ItemProperty>) {
    super(init);
    this.characteristics = characteristics;
    this.characteristicValues = characteristicValues;
    if (init && init.characteristicValueId) {
      this.updateValues(init);
    }
    else {
      this.characteristicId = 0;
      this.characteristicValue = new CharacteristicValue();
    }
  }
  updateValues(value?: Partial<ItemProperty>) {
    this.id = value.id;
    this.characteristicValueId = value.characteristicValueId;
    this.itemVariantId = value.itemVariantId;
    this.characteristicValue = this.characteristicValues.find(c => c.id == value.characteristicValueId);
    this.characteristicId = this.characteristicValue.characteristicId;
  }
  getCharacteristics(): Characteristic[] {
    return this.characteristics;
  }
  getCharacteristicValues(): CharacteristicValue[] {
    return this.characteristicValues.filter(c => c.characteristicId == this.characteristicId);
  }
}
