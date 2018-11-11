import { Component, OnInit } from '@angular/core';
import { CartItemService } from '../../services/cart-item.service';
import { CartItemThumbnail } from '../../view-models/cart-item/cart-item-thumbnail';
import { Item } from '../../view-models/item/item';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import { ParameterNames } from '../../services/parameter-names';
import { ParameterService } from '../../services/parameter.service';
import { CartItem } from '../../view-models/cart-item/cart-item';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  protected cartItemThumbnails: CartItemThumbnail[] = [];

  constructor(
    private cartItemService: CartItemService,
    private parameterService: ParameterService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  protected initializeComponent() {
    this.cartItemService.cartItemThumbnails$.subscribe(
      (thumnails: CartItemThumbnail[]) => {
        this.cartItemThumbnails = thumnails;
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
    this.cartItemService.addToCart(new CartItem({ itemVariantId: itemVariant.id, number: 1 }));
  }

  minusCartItem(itemVariant: ItemVariant) {
    this.cartItemService.removeFromCart(new CartItem({ itemVariantId: itemVariant.id, number: 1 }));
  }

  removeCartItem(cartItemThumbnail: CartItemThumbnail) {
    this.cartItemService.removeFromCart(new CartItem({ itemVariantId: cartItemThumbnail.itemVariant.id, number: cartItemThumbnail.number }));
  }
}
