import { Component, OnInit } from '@angular/core';
import { CartItemThumbnail } from '../cart-item-thumbnail';
import { CartItemService } from '../cart-item.service';
import { ItemVariant } from 'src/app/item-variant/item-variant';
import { CartItem } from '../cart-item';
import { Router } from '@angular/router';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { MessageService } from '../../../message/message.service';

@Component({
  selector: 'app-cart-item-thumbnail-index',
  templateUrl: './cart-item-thumbnail-index.component.html',
  styleUrls: ['./cart-item-thumbnail-index.component.css']
})
export class CartItemThumbnailIndexComponent implements OnInit {
  protected cartItemThumbnails: CartItemThumbnail[] = [];
  protected totalPrice: number = 0;
  protected totalQuantity: number = 0;

  constructor(
    private cartItemService: CartItemService,
    private messageService: MessageService,
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
        this.totalPrice = 0;
        this.totalQuantity = 0;
        this.cartItemThumbnails.forEach(c => {
          this.totalPrice += c.number * c.itemVariant.price;
          this.totalQuantity += c.number;
        });
      }
    );
    this.cartItemService.updateCartItemThumbnails();

    //localStorage.removeItem('cart-item-thumbnail-index-help-shown');
    if (!localStorage.getItem('cart-item-thumbnail-index-help-shown')) {
      this.showHelpMessages();
      localStorage.setItem('cart-item-thumbnail-index-help-shown', 'true');
    }
  }

  showHelpMessages() {
    this.messageService.delay(1 * 1000).then(() =>
      this.messageService.sendHelp("All added items and available actions displayed here. Creating order will move them to orders. "));
  }

  public getItemUrl(cartItemThumbnail: CartItemThumbnail) {
    let itemUrl = `/items/${cartItemThumbnail.itemVariant.itemId}/detail/`;
    return itemUrl;
  }

  public getItemVariantParams(itemVariant: ItemVariant) {
    let params = this.parameterService.getUpdatedParamsCopy({ "itemVariantId": itemVariant.id });
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
