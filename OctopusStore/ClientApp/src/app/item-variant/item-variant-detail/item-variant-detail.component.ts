import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { ItemVariant } from '../../view-models/item-variant';
import { ItemDetail } from '../../view-models/item-detail';

@Component({
  selector: 'app-item-variant-detail',
  templateUrl: './item-variant-detail.component.html',
  styleUrls: ['./item-variant-detail.component.css']
})
export class ItemVariantDetailComponent implements OnInit, OnChanges {
  @Input() itemDetail: ItemDetail;
  public currentVariant: ItemVariant;
  @Output() itemVariantSelected = new EventEmitter<ItemVariant>();

  constructor() { }

  ngOnInit() {

  }

  ngOnChanges() {
    if (this.itemDetail && this.itemDetail.itemVariants) {
      this.itemDetail = this.itemDetail;
      if (this.itemDetail.itemVariants[0]) {
        this.selectItemVariant(this.itemDetail.itemVariants[0]);
      }
    }
  }

  selectItemVariant(itemVariant: ItemVariant) {
    this.currentVariant = this.itemDetail.itemVariants.find(v => v == itemVariant);
    this.itemVariantSelected.emit(itemVariant);
  }
}
