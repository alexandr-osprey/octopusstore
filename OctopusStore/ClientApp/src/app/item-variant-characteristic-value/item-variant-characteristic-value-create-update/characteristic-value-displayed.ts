import { Characteristic } from "../../view-models/characteristic/characteristic";
import { CharacteristicValue } from "../../view-models/characteristic-value/characteristic-value";
import { ItemVariantCharacteristicValue } from "../../view-models/item-variant-characteristic-value/item-variant-characteristic-value";
import { ItemVariant } from "../../view-models/item-variant/item-variant";

export class CharacteristicValueDisplayed extends ItemVariantCharacteristicValue {
  public static characteristics: Characteristic[];
  public static characteristicValues: CharacteristicValue[];
  private _characteristicId: number;

  public set characteristicId(characteristicId: number) {
    this._characteristicId = characteristicId;
    this.characteristic = CharacteristicValueDisplayed.characteristics.find(c => c.id == characteristicId);
  }
  public get characteristicId(): number {
    return this._characteristicId;
  }
  public characteristicValue: CharacteristicValue;
  public characteristic: Characteristic;
  public itemVariant: ItemVariant;

  constructor(init?: Partial<ItemVariantCharacteristicValue>) {
    super(init);
    if (init.characteristicValueId) {
      this.characteristicValue = CharacteristicValueDisplayed.characteristicValues.find(c => c.id == init.characteristicValueId);
      this.characteristicId = this.characteristicValue.characteristicId;
    }
    else {
      this.characteristicId = 0;
      this.characteristicValue = new CharacteristicValue();
    }
  }
  getCharacteristics(): Characteristic[] {
    return CharacteristicValueDisplayed.characteristics;
  }
  getCharacteristicValues(): CharacteristicValue[] {
    return CharacteristicValueDisplayed.characteristicValues.filter(c => c.characteristicId == this.characteristicId);
  }
}
