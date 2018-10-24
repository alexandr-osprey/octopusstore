import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ItemVariantCharacteristicValue } from '../../view-models/item-variant-characteristic-value/item-variant-characteristic-value';
import { MessageService } from '../../services/message.service';
import { CharacteristicService } from '../../services/characteristic.service';
import { CharacteristicValueService } from '../../services/characteristic-value.service';
import { ItemVariantCharacteristicValueService } from '../../services/item-variant-characteristic-value-service';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { Observable } from 'rxjs';
import 'rxjs/add/observable/zip';
import { CharacteristicValueDisplayed } from './characteristic-value-displayed';
import { Item } from '../../view-models/item/item';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';
import { Characteristic } from '../../view-models/characteristic/characteristic';

@Component({
  selector: 'app-item-variant-characteristic-value-create-update',
  templateUrl: './item-variant-characteristic-value-create-update.component.html',
  styleUrls: ['./item-variant-characteristic-value-create-update.component.css']
})
export class ItemVariantCharacteristicValueCreateUpdateComponent implements OnInit, OnChanges {
  @Input() itemVariant: ItemVariant;
  @Input() item: Item;
  public characteristicValuesDisplayed: CharacteristicValueDisplayed[] = [];
  public itemCharacteristicValues: CharacteristicValueDisplayed[] = [];

  public characteristics: Characteristic[] = [];
  public characteristicValues: CharacteristicValue[] = [];

  constructor(
    protected messageService: MessageService,
    protected characteristicService: CharacteristicService,
    protected characteristicValueService: CharacteristicValueService,
    protected itemVariantCharacteristicValueService: ItemVariantCharacteristicValueService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }
  ngOnChanges(changes: SimpleChanges) {
    this.currentVariantChanged();
  }
  protected initializeComponent() {
    if (this.item && this.item.id) {
      Observable.zip(
        this.characteristicService.index({ categoryId: this.item.categoryId }),
        this.characteristicValueService.index({ categoryId: this.item.categoryId }),
        this.itemVariantCharacteristicValueService.index({ itemId: this.item.id })
      ).subscribe(data => {
        
        data[0].entities.forEach(c => this.characteristics.push(new Characteristic(c)));
        data[1].entities.forEach(cv => this.characteristicValues.push(new CharacteristicValue(cv)));
        data[2].entities.forEach(i => {
          this.itemCharacteristicValues.push(new CharacteristicValueDisplayed(this.characteristics, this.characteristicValues, i));
        });
        this.currentVariantChanged();
      });
    }
  }
  protected currentVariantChanged() {
    this.characteristicValuesDisplayed = this.itemCharacteristicValues.filter(v => v.itemVariantId == this.itemVariant.id);
  }
  protected addItemVariantCharacteristicValue() {
    let addedItemVariant = new CharacteristicValueDisplayed(this.characteristics, this.characteristicValues, { id: 0, itemVariantId: this.itemVariant.id });
    this.itemCharacteristicValues.push(addedItemVariant);
    this.characteristicValuesDisplayed.push(addedItemVariant);
  }
  protected removeItemVariantCharacteristicValue(characteristicValueDisplayed: CharacteristicValueDisplayed) {
    if (characteristicValueDisplayed.id != 0) {
      this.itemVariantCharacteristicValueService.delete(characteristicValueDisplayed.id).subscribe(data => {
        if (data) {
          this.characteristicValuesDisplayed = this.characteristicValuesDisplayed.filter(c => c.id != characteristicValueDisplayed.id);
          this.messageService.sendSuccess("Item variant characteristic value deleted");
        }
      });
    }
    this.characteristicValuesDisplayed = this.characteristicValuesDisplayed.filter(v => v != characteristicValueDisplayed);
    this.itemCharacteristicValues = this.itemCharacteristicValues.filter(v => v != characteristicValueDisplayed)
  }
  protected saveItemVariantCharacteristicValues() {
    this.characteristicValuesDisplayed.forEach(value => {
      this.saveItemVariantCharacteristicValue(value);
    });
    this.messageService.sendSuccess("Item variant characteristic values saved");
  }
  protected saveItemVariantCharacteristicValue(value: CharacteristicValueDisplayed) {
    let index = this.characteristicValuesDisplayed.indexOf(value);
    if (~index) {
      this.itemVariantCharacteristicValueService.postOrPut(value).subscribe((data: ItemVariantCharacteristicValue) => {
        if (data) {
          this.characteristicValuesDisplayed[index].updateValues(data);
        }
      });
    }
  }
}
