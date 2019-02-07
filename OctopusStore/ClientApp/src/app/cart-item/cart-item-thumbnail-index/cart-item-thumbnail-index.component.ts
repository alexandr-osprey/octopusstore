import { Component, OnInit } from '@angular/core';
import { ParameterNames } from '../../parameter/parameter-names';
import { ParameterService } from '../../parameter/parameter.service';
import { CartItemThumbnail } from '../cart-item-thumbnail';
import { CartItemService } from '../cart-item.service';
import { Item } from 'src/app/item/item';
import { ItemVariant } from 'src/app/item-variant/item-variant';
import { CartItem } from '../cart-item';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-item-thumbnail-index',
  templateUrl: './cart-item-thumbnail-index.component.html',
  styleUrls: ['./cart-item-thumbnail-index.component.css']
})
export class CartItemThumbnailIndexComponent implements OnInit {
  protected cartItemThumbnails: CartItemThumbnail[] = [];
  protected sum: number = 0;

  constructor(
    private cartItemService: CartItemService,
    private router: Router,
    private parameterService: ParameterService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  protected initializeComponent() {
    this.cartItemService.cartItemThumbnails$.subscribe(
      (thumnails: CartItemThumbnail[]) => {
        this.cartItemThumbnails = thumnails;
        this.sum = 0;
        this.cartItemThumbnails.forEach(c => {
          this.sum += c.number * c.itemVariant.price;
        });
      }
    );
    this.cartItemService.updateCartItemThumbnails();
  }

  public getItemUrl(item: Item) {
    let itemUrl = `/items/${item.id}/detail/`;
    return itemUrl;
  }

  public getItemVariantParams(itemVariant: ItemVariant) {
    let params = this.parameterService.getUpdatedParams([ParameterNames.itemVariantId, itemVariant.id]);
    return params;
  }

  plusCartItem(itemVariant: ItemVariant) {
    let item = new CartItem({ itemVariantId: itemVariant.id, number: 1 });
    this.cartItemService.addToCart(item);
  }

  minusCartItem(itemVariant: ItemVariant) {
    let item = new CartItem({ itemVariantId: itemVariant.id, number: 1 });
    this.cartItemService.removeFromCart(item);
  }

  setCartItemThumbnail(cartItemThumbnail: CartItemThumbnail) {
    let item = new CartItem({ itemVariantId: cartItemThumbnail.itemVariant.id, number: cartItemThumbnail.number });
    this.cartItemService.setCartItem(item);
  }

  removeCartItem(cartItemThumbnail: CartItemThumbnail) {
    let item = new CartItem({ itemVariantId: cartItemThumbnail.itemVariant.id, number: cartItemThumbnail.number });
    this.cartItemService.removeFromCart(item);
  }

  createOrder() {
    this.router.navigate(['/orders/create']);
  }
}
