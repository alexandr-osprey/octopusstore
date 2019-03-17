import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ItemDetail } from '../item-detail';
import { ItemService } from '../item.service';
import { ItemVariant } from 'src/app/item-variant/item-variant';
import { ItemImage } from 'src/app/item-image/item-image';
import { IdentityService } from 'src/app/identity/identity.service';
import { CartItem } from 'src/app/cart/cart-item/cart-item';
import { CartItemService } from 'src/app/cart/cart-item/cart-item.service';

@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.component.html',
  styleUrls: ['./item-detail.component.css'],
  providers: [ItemService]
})
export class ItemDetailComponent implements OnInit {
  public itemDetail: ItemDetail;
  public authorizedToUpdate: boolean = false;
  public currentVariant: ItemVariant;

  public displayedImages: ItemImage[];
  public currentPrice: number;

  constructor(
    private itemService: ItemService,
    private route: ActivatedRoute,
    private identityService: IdentityService,
    private cartItemService: CartItemService
  ) {
  }

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
          this.authorizedToUpdate = this.identityService.checkUpdateAuthorization(data.store.id);
        }
      });
    }
  }

  addToCart() {
    this.cartItemService.addToCart(new CartItem({ itemVariantId: this.currentVariant.id, number: 1 }));
  }

  removeFromCart() {
    this.cartItemService.removeFromCart(new CartItem({ itemVariantId: this.currentVariant.id, number: 1 }));
  }

  getUpdateLink(id: number): string {
    return this.itemService.getUrlWithIdWithSuffix(id, 'update', '/items');
  }
  itemVariantSelected(selectedItemVariant: ItemVariant) {
    this.currentVariant = selectedItemVariant;
    this.currentPrice = selectedItemVariant.price;
  }
}
