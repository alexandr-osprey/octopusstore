import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ItemPropertyService } from '../../services/item-property.service';
import { ItemDetail } from '../../view-models/item/item-detail';
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/zip";
import { MessageService } from '../../services/message.service';
import { CharacteristicService } from '../../services/characteristic.service';
import { CharacteristicValueService } from '../../services/characteristic-value.service';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';
import { CharacteristicValueDisplayed } from '../../view-models/characteristic-value/characteristic-value-displayed';

@Component({
  selector: 'app-item-property-detail',
  templateUrl: './item-property-detail.component.html',
  styleUrls: ['./item-property-detail.component.css']
})
export class ItemPropertyDetailComponent implements OnInit, OnChanges {
  @Input() itemDetail: ItemDetail;
  @Input() currentVariant: ItemVariant;
  public characteristicValuesDisplayed: CharacteristicValueDisplayed[];
  public itemCharacteristicValues: CharacteristicValueDisplayed[];

  constructor(
    private messageService: MessageService,
    private characteristicService: CharacteristicService,
    private characteristicValueService: CharacteristicValueService,
    private itemPropertyService: ItemPropertyService) {
    this.itemCharacteristicValues = [];
  }

  ngOnInit() {
    this.initializeComponent();
  }
  ngOnChanges(changes: SimpleChanges) {
    this.currentVariantChanged();
  }

  initializeComponent() {
    if (this.itemDetail) {
      Observable.zip(
        this.characteristicService.index({ categoryId: this.itemDetail.category.id }),
        this.characteristicValueService.index({ categoryId: this.itemDetail.category.id }),
        this.itemPropertyService.index({ itemId: this.itemDetail.id })
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
