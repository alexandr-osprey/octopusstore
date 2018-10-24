import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ItemDetails } from '../../view-models/item/item-details';
import { ItemService } from '../../services/item.service';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { ItemImage } from '../../view-models/item-image/item-image';
import { IdentityService } from '../../services/identity-service';

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css'],
  providers: [ItemService]
})
export class ItemDetailsComponent implements OnInit {
  public itemDetails: ItemDetails;
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
      this.itemService.getDetail(itemId).subscribe((data: ItemDetails) => {
        if (data) {
          this.itemDetails = new ItemDetails(data);
          this.displayedImages = this.itemDetails.images;
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
