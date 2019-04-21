import { Component, OnInit, AfterContentInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ItemDetail } from '../item-detail';
import { ItemService } from '../item.service';
import { ItemVariant } from 'src/app/item-variant/item-variant';
import { IdentityService } from 'src/app/identity/identity.service';
import { CartItem } from 'src/app/cart/cart-item/cart-item';
import { CartItemService } from 'src/app/cart/cart-item/cart-item.service';
import { CartItemThumbnail } from 'src/app/cart/cart-item/cart-item-thumbnail';
import { ItemVariantImage } from 'src/app/item-variant-image/item-variant-image';
import { ParameterService } from 'src/app/parameter/parameter.service';

@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.component.html',
  styleUrls: ['./item-detail.component.css'],
  providers: [ItemService]
})
export class ItemDetailComponent implements OnInit, AfterContentInit {
  public itemDetail: ItemDetail;
  public authorizedToUpdate: boolean = false;
  public currentVariant: ItemVariant;
  public shouldShowRemoveFromCart: boolean;
  public numberInCart: number;
  public backPath: string;
  public backParams: any;

  //public displayedImages: ItemVariantImage[];
  public currentPrice: number;
  public loaded: boolean;

  constructor(
    private itemService: ItemService,
    private route: ActivatedRoute,
    private identityService: IdentityService,
    private cartItemService: CartItemService,
    protected parameterService: ParameterService,
  ) {
  }

  ngOnInit(): void {
    this.initializeComponent();
    
  }

  initializeComponent() {
    this.backPath = this.parameterService.oldPath;
    this.backParams = this.parameterService.oldParams;
    let itemId = +this.route.snapshot.paramMap.get('id');
    if (itemId) {
      this.itemService.getDetail(itemId).subscribe((data: ItemDetail) => {
        if (data) {
          this.itemDetail = new ItemDetail(data);
          //this.displayedImages = this.itemDetail.images;
          this.authorizedToUpdate = this.identityService.checkUpdateAuthorization(data.store.id);
        }
      });
    }
    this.cartItemService.cartItemThumbnails$.subscribe((thumbnails: CartItemThumbnail[]) => {
      this.setCartInfo();
    });
  }

  ngAfterContentInit() {
    let self = this;
    //setTimeout(() => {
      //self.loaded = true;
    //}, 5);
    this.itemService.delay(500).then(() => {
      this.loaded = true;
      this.setCartInfo();
    })
    
  }

  setCartInfo() {
    if (!this.currentVariant)
      return false;
    this.shouldShowRemoveFromCart = this.cartItemService.cartItemThumbnails.some(c => c.itemVariantId == this.currentVariant.id);
    let inCart = this.cartItemService.cartItemThumbnails.filter(c => c.itemVariantId == this.currentVariant.id)[0];
    if (inCart) {
      this.numberInCart = inCart.number;
    } else {
      this.numberInCart = 0;
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
    if (this.loaded) {
      this.setCartInfo();
    }
  }
}
