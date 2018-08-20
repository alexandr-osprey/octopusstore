import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Item } from '../../view-models/item/item';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { ItemVariantService } from '../../services/item-variant.service';
import { ItemVariantIndex } from '../../view-models/item-variant/item-variant-index';
import { MessageService } from '../../services/message.service';
import { ItemVariantCharacteristicValueCreateUpdateComponent } from '../../item-variant-characteristic-value/item-variant-characteristic-value-create-update/item-variant-characteristic-value-create-update.component';


@Component({
  selector: 'app-item-variant-create-update',
  templateUrl: './item-variant-create-update.component.html',
  styleUrls: ['./item-variant-create-update.component.css']
})
export class ItemVariantCreateUpdateComponent implements OnInit {
  @Input() item: Item;
  public currentVariant: ItemVariant;
  public itemVariants: ItemVariant[];

  public try: number = 3;

  constructor(
    private messageService: MessageService,
    private itemVariantService: ItemVariantService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    if (this.item.id) {
      this.itemVariants = [];
      this.currentVariant = this.itemVariants[0];
      this.itemVariantService.index({ itemId: this.item.id }).subscribe((data: ItemVariantIndex) => {
        this.itemVariants = data.entities;
        this.selectItemVariant(this.itemVariants[0]);
      });
    }
  }
  addItemVariant(): ItemVariant {
    let newVariant = new ItemVariant({
      id: 0,
      title: "new variant",
      price: 0,
      itemId: this.item.id
    });
    this.itemVariants.push(newVariant);
    return newVariant;
  }
  selectItemVariant(itemVariant: ItemVariant) {
    if (this.itemVariants) {
      this.currentVariant = itemVariant;
    }
  }

  saveItemVariant() {
    let i: number = this.itemVariants.indexOf(this.currentVariant);
    this.itemVariantService.createOrUpdate(this.currentVariant).subscribe((data: ItemVariant) => {
      this.itemVariants[i] = data;
      this.currentVariant = data;
    });
  }
  deleteItemVariant() {
    this.itemVariantService.delete(this.currentVariant.id).subscribe((data: any) => {
      this.itemVariants = this.itemVariants.filter(v => v != this.currentVariant);
      if (this.itemVariants[0])
        this.currentVariant = this.itemVariants[0];
      else
        this.currentVariant = this.addItemVariant();
    });
  }

  getUpdatedParams(paramName: string, param: number): any {
    var params: any = {};
    params[paramName] = param;
    return params;
  }
}
