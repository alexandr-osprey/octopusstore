import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ItemDetail } from '../../view-models/item/item-detail';
import { ItemService } from '../../services/item.service';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { ItemImage } from '../../view-models/item-image/item-image';
import { IdentityService } from '../../services/identity-service';

@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.component.html',
  styleUrls: ['./item-detail.component.css'],
  providers: [ItemService]
})
export class ItemDetailComponent implements OnInit {
  public itemDetail: ItemDetail;
  public authorizedToUpdate: boolean = false;

  public displayedImages: ItemImage[];
  public currentPrice: number;

  constructor(
    private itemService: ItemService,
    private route: ActivatedRoute,
    private identityService: IdentityService
  ) { }

  ngOnInit(): void {
    this.initializeComponent();
  }

  initializeComponent() {
    let itemId = +this.route.snapshot.paramMap.get('id');
    if (itemId) {
      this.itemService.getDetail(itemId).subscribe((data: ItemDetail) => {
        if (data) {
          this.itemDetail = new ItemDetail(data);
          this.displayedImages = this.itemDetail.images;
        }
        this.identityService.checkCreateUpdateAuthorization(this.getUpdateLink(itemId)).subscribe(result => {
          if (result)
            this.authorizedToUpdate = true;
        });
      });
    }
  }

  getUpdateLink(id: number): string {
    return this.itemService.getUrlWithIdWithSuffix(id, 'update', '/items');
  }
  itemVariantSelected(selectedItemVariant: ItemVariant) {
    this.currentPrice = selectedItemVariant.price;
  }
}
