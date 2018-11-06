import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { IdentityService } from './identity.service';
import { DataReadWriteService } from './data-read-write.service';
import { Router } from '@angular/router';
import { CartItem } from '../view-models/cart-item/cart-item';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartItemService extends DataReadWriteService<CartItem> {
  protected cartItems: any = {};
  protected paramsSource = new Subject<any>();
  public params$ = this.paramsSource.asObservable();

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/cartItems';
    this.serviceName = 'Cart items service';
  }

  public addToCart(cartItem: CartItem) {

  }
}
