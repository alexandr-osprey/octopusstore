import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { MessageService } from '../../services/message.service';
import { CharacteristicService } from '../../services/characteristic.service';
import { CharacteristicValueService } from '../../services/characteristic-value.service';
import { ItemPropertyService } from '../../services/item-property-service';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { Observable } from 'rxjs';
import 'rxjs/add/observable/zip';
import { CharacteristicValueDisplayed } from './characteristic-value-displayed';
import { Item } from '../../view-models/item/item';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { ItemProperty } from '../../view-models/item-property/item-property';

@Component({
  selector: 'app-item-property-create-update',
  templateUrl: './item-property-create-update.component.html',
  styleUrls: ['./item-property-create-update.component.css']
})
export class ItemPropertyCreateUpdateComponent implements OnInit, OnChanges {
  @Input() itemVariant: ItemVariant;
  @Input() item: Item;
  public itemPropertiesSaved: CharacteristicValueDisplayed[] = [];
  public itemPropertiesUnsaved: CharacteristicValueDisplayed[] = [];
  public itemCharacteristicValues: CharacteristicValueDisplayed[] = [];

  public characteristics: Characteristic[] = [];
  public characteristicValues: CharacteristicValue[] = [];

  constructor(
    protected messageService: MessageService,
    protected characteristicService: CharacteristicService,
    protected characteristicValueService: CharacteristicValueService,
    protected itemPropertyService: ItemPropertyService) {
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
        this.itemPropertyService.index({ itemId: this.item.id })
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
    this.itemPropertiesSaved = this.itemCharacteristicValues.filter(v => v.itemVariantId == this.itemVariant.id && v.id != 0);
    this.itemPropertiesUnsaved = this.itemCharacteristicValues.filter(v => v.itemVariantId == this.itemVariant.id && v.id == 0);
  }
  protected addItemProperty() {
    let addedItemVariant = new CharacteristicValueDisplayed(this.characteristics, this.characteristicValues, { id: 0, itemVariantId: this.itemVariant.id });
    this.itemCharacteristicValues.push(addedItemVariant);
    this.itemPropertiesUnsaved.push(addedItemVariant);
  }
  protected removeItemProperty(itemPropertyDisplayed: CharacteristicValueDisplayed) {
    if (itemPropertyDisplayed.id != 0) {
      this.itemPropertyService.delete(itemPropertyDisplayed.id).subscribe(data => {
        if (data) {
          this.itemPropertiesSaved = this.itemPropertiesSaved.filter(c => c.id != itemPropertyDisplayed.id);
          this.messageService.sendSuccess("Item variant characteristic value deleted");
        }
      });
    }
    this.itemPropertiesSaved = this.itemPropertiesSaved.filter(v => v != itemPropertyDisplayed);
    this.itemPropertiesUnsaved = this.itemPropertiesUnsaved.filter(v => v != itemPropertyDisplayed);
    this.itemCharacteristicValues = this.itemCharacteristicValues.filter(v => v != itemPropertyDisplayed)
  }
  protected saveItemProperties() {
    this.itemPropertiesUnsaved.forEach(value => this.saveItemProperty(value));
    //this.messageService.sendSuccess("Item variant characteristic values saved");
  }
  protected saveItemProperty(value: CharacteristicValueDisplayed) {
    let index = this.itemPropertiesUnsaved.indexOf(value);
    if (~index) {
      this.itemPropertyService.postOrPut(value).subscribe((data: ItemProperty) => {
        if (data) {
          this.itemPropertiesUnsaved = this.itemPropertiesUnsaved.filter(v => v != value);
          this.itemCharacteristicValues = this.itemCharacteristicValues.filter(v => v != value);
          let savedValue = new CharacteristicValueDisplayed(this.characteristics, this.characteristicValues, data);
          this.itemPropertiesSaved.push(savedValue);
          this.itemCharacteristicValues.push(savedValue);
        }
      });
    }
  }
}
