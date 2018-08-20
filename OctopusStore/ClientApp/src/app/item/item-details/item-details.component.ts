import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ItemDetail } from '../../view-models/item/item-detail';
import { ItemService } from '../../services/item.service';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { ItemImage } from '../../view-models/item-image/item-image';
import { Item } from '../../view-models/item/item';

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css'],
  providers: [ItemService]
})
export class ItemDetailsComponent implements OnInit {
  public itemDetails: ItemDetail;
  public displayedImages: ItemImage[];
  public currentPrice: number;

  constructor(
    private itemService: ItemService,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.initializeComponent();
  }

  initializeComponent() {
    let itemId = +this.route.snapshot.paramMap.get('id');
    if (itemId) {
      this.itemService.getDetail(itemId).subscribe((data: ItemDetail) => {
        //this.itemDetails = Object.assign(new ItemDetail(), data);
        this.itemDetails = data;
        this.displayedImages = this.itemDetails.images;
      });
    }
  }
  goBack(): void {
     this.location.back();
  }
  itemVariantSelected(selectedItemVariant: ItemVariant) {
    this.currentPrice = selectedItemVariant.price;
  }
}
