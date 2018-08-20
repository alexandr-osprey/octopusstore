import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';
import { ItemVariantCharacteristicValue } from '../../view-models/item-variant-characteristic-value/item-variant-characteristic-value';
import { MessageService } from '../../services/message.service';
import { CharacteristicService } from '../../services/characteristic.service';
import { CharacteristicValueService } from '../../services/characteristic-value.service';
import { ItemVariantCharacteristicValueService } from '../../services/item-variant-characteristic-value-service';
import { CharacteristicIndex } from '../../view-models/characteristic/characteristic-index';
import { CharacteristicValueIndex } from '../../view-models/characteristic-value/characteristic-value-index';
import { ItemVariantCharacteristicValueIndex } from '../../view-models/item-variant-characteristic-value/item-variant-characteristic-value-index';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { Observable, observable, from } from 'rxjs';
import 'rxjs/add/observable/zip';
import { CharacteristicValueDisplayed } from './characteristic-value-displayed';
import { Item } from '../../view-models/item/item';
import { forEach } from '@angular/router/src/utils/collection';


@Component({
  selector: 'app-item-variant-characteristic-value-create-update',
  templateUrl: './item-variant-characteristic-value-create-update.component.html',
  styleUrls: ['./item-variant-characteristic-value-create-update.component.css']
})
export class ItemVariantCharacteristicValueCreateUpdateComponent implements OnInit, OnChanges {

  @Input() itemVariant: ItemVariant;
  @Input() item: Item;
  public characteristicValuesDisplayed: CharacteristicValueDisplayed[];
  public itemCharacteristicValues: CharacteristicValueDisplayed[];

  constructor(
    private messageService: MessageService,
    private characteristicService: CharacteristicService,
    private characteristicValueService: CharacteristicValueService,
    private itemVariantCharacteristicValueService: ItemVariantCharacteristicValueService) {
    this.itemCharacteristicValues = [];
  }

  ngOnInit() {
    this.initializeComponent();
  }
  ngOnChanges(changes: SimpleChanges) {
    this.currentVariantChanged();
  }
  initializeComponent() {
    if (this.item.id) {
      Observable.zip(
        this.characteristicService.index({ categoryId: this.item.categoryId }),
        this.characteristicValueService.index({ categoryId: this.item.categoryId }),
        this.itemVariantCharacteristicValueService.index({ itemId: this.item.id }))
        .subscribe(data => {
          CharacteristicValueDisplayed.characteristics = data[0].entities;
          CharacteristicValueDisplayed.characteristicValues = data[1].entities;
          this.itemCharacteristicValues = data[2].entities.map(function (v) {
            return new CharacteristicValueDisplayed(v)
          });
          this.currentVariantChanged();
        });
    }
  }
  currentVariantChanged() {
    this.characteristicValuesDisplayed = this.itemCharacteristicValues.filter(v => v.itemVariantId == this.itemVariant.id);
  }
  addItemVariantCharacteristicValue() {
    let addedItemVariant = new CharacteristicValueDisplayed({ id: 0, itemVariantId: this.itemVariant.id });
    this.itemCharacteristicValues.push(addedItemVariant);
    this.characteristicValuesDisplayed.push(addedItemVariant);
  }
  removeItemVariantCharacteristicValue(characteristicValueDisplayed: CharacteristicValueDisplayed) {
    if (characteristicValueDisplayed.id != 0) {
      this.itemVariantCharacteristicValueService.delete(characteristicValueDisplayed.id).subscribe(_ => {
        this.characteristicValuesDisplayed = this.characteristicValuesDisplayed.filter(v => v != characteristicValueDisplayed);
        this.itemCharacteristicValues = this.itemCharacteristicValues.filter(v => v != characteristicValueDisplayed);
      });
    }
  }
  saveItemVariantCharacteristicValues() {
    let observables = this.characteristicValuesDisplayed.map(function (v): Observable<ItemVariantCharacteristicValue> {
      return this.itemVariantCharacteristicValueService.createOrUpdate(v);
    }, this)
    Observable.zip(...observables).subscribe((data: ItemVariantCharacteristicValue[]) => {
      this.itemCharacteristicValues = this.itemCharacteristicValues.filter(v => !this.characteristicValuesDisplayed.includes(v));
      let updated = data.map(function (v) {
        return new CharacteristicValueDisplayed(v);
      });
      this.characteristicValuesDisplayed = updated;
      this.itemCharacteristicValues.push.apply(this.itemCharacteristicValues, updated);
    });
  }
}
