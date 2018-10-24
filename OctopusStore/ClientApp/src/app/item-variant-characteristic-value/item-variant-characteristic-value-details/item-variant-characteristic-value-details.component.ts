import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ItemVariantCharacteristicValueService } from '../../services/item-variant-characteristic-value-service';
import { ItemDetails } from '../../view-models/item/item-details';
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/zip";
import { MessageService } from '../../services/message.service';
import { CharacteristicService } from '../../services/characteristic.service';
import { CharacteristicValueService } from '../../services/characteristic-value.service';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { CharacteristicValueDisplayed } from '../item-variant-characteristic-value-create-update/characteristic-value-displayed';
import { Subscription } from 'rxjs';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';

@Component({
  selector: 'app-item-variant-characteristic-value-details',
  templateUrl: './item-variant-characteristic-value-details.component.html',
  styleUrls: ['./item-variant-characteristic-value-details.component.css']
})
export class ItemVariantCharacteristicValueDetailsComponent implements OnInit, OnChanges {
  @Input() itemDetails: ItemDetails;
  @Input() currentVariant: ItemVariant;
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
    if (this.itemDetails) {
      Observable.zip(
        this.characteristicService.index({ categoryId: this.itemDetails.category.id }),
        this.characteristicValueService.index({ categoryId: this.itemDetails.category.id }),
        this.itemVariantCharacteristicValueService.index({ itemId: this.itemDetails.id })
      ).subscribe((data) => {
        let characteristics: Characteristic[] = [];
        let characteristicValues: CharacteristicValue[] = [];
        data[0].entities.forEach(c => characteristics.push(new Characteristic(c)));
        data[1].entities.forEach(cv => characteristicValues.push(new CharacteristicValue(cv)));
        for (var v of data[2].entities) {
          this.itemCharacteristicValues.push(new CharacteristicValueDisplayed(characteristics, characteristicValues, v));
        }
        this.currentVariantChanged();
      });
    }
  }
  currentVariantChanged() {
    this.characteristicValuesDisplayed = this.itemCharacteristicValues.filter(v => v.itemVariantId == this.currentVariant.id);
  }
}
