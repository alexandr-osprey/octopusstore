import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Observable } from 'rxjs';
import { ItemDetail } from 'src/app/item/item-detail';
import { ItemVariant } from 'src/app/item-variant/item-variant';
import { ItemPropertyDisplayed } from '../item-property-displayed';
import { MessageService } from 'src/app/message/message.service';
import { CharacteristicService } from 'src/app/characteristic/characteristic.service';
import { CharacteristicValueService } from 'src/app/characteristic-value/characteristic-value.service';
import { ItemPropertyService } from '../item-property.service';
import { Characteristic } from 'src/app/characteristic/characteristic';
import { CharacteristicValue } from 'src/app/characteristic-value/characteristic-value';

@Component({
  selector: 'app-item-property-detail',
  templateUrl: './item-property-detail.component.html',
  styleUrls: ['./item-property-detail.component.css']
})
export class ItemPropertyDetailComponent implements OnInit, OnChanges {
  @Input() itemDetail: ItemDetail;
  @Input() currentVariant: ItemVariant;
  public characteristicValuesDisplayed: ItemPropertyDisplayed[];
  public itemCharacteristicValues: ItemPropertyDisplayed[];

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
          this.itemCharacteristicValues.push(new ItemPropertyDisplayed(characteristics, characteristicValues, v));
        }
        this.currentVariantChanged();
      });
    }
  }
  currentVariantChanged() {
    this.characteristicValuesDisplayed = this.itemCharacteristicValues.filter(v => v.itemVariantId == this.currentVariant.id);
  }
}
