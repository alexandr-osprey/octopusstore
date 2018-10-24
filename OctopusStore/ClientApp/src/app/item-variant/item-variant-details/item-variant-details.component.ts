import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { ItemDetails } from '../../view-models/item/item-details';

@Component({
  selector: 'app-item-variant-details',
  templateUrl: './item-variant-details.component.html',
  styleUrls: ['./item-variant-details.component.css']
})
export class ItemVariantDetailsComponent implements OnInit, OnChanges {
  @Input() itemDetails: ItemDetails;
  public currentVariant: ItemVariant;
  @Output() itemVariantSelected = new EventEmitter<ItemVariant>();

  constructor() { }

  ngOnInit() {

  }

  ngOnChanges() {
    if (this.itemDetails && this.itemDetails.itemVariants) {
      this.itemDetails = this.itemDetails;
      if (this.itemDetails.itemVariants[0]) {
        this.selectItemVariant(this.itemDetails.itemVariants[0]);
      }
    }
  }

  selectItemVariant(itemVariant: ItemVariant) {
    this.currentVariant = this.itemDetails.itemVariants.find(v => v == itemVariant);
    this.itemVariantSelected.emit(itemVariant);
  }
}
