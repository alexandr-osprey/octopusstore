import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { IdentityService } from './identity.service';
import { DataReadWriteService } from './data-read-write.service';
import { Router } from '@angular/router';
import { CartItem } from '../view-models/cart-item/cart-item';
import { Subject, Observable } from 'rxjs';
import { ParameterNames } from './parameter-names';
import { CartItemThumbnail } from '../view-models/cart-item/cart-item-thumbnail';
import { EntityIndex } from '../view-models/entity/entity-index';
import { ItemService } from './item.service';
import { ItemVariantService } from './item-variant.service';
import { ItemVariant } from '../view-models/item-variant/item-variant';
import { Item } from '../view-models/item/item';
import { Response } from '../view-models/response';

@Injectable({
  providedIn: 'root'
})
export class CartItemService extends DataReadWriteService<CartItem> {
  protected localStorageKey: string = "cartItemLocalStorage";
  protected cartItemThumbnails: CartItemThumbnail[] = [];
  protected cartItemThumbnailsSource = new Subject<any>();
  public cartItemThumbnails$ = this.cartItemThumbnailsSource.asObservable();

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected itemService: ItemService,
    protected itemVariantService: ItemVariantService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/cartItems';
    this.serviceName = 'Cart items service';
    this.getAuthenticationRequired = true;

    this.initialize();
  }

  protected initialize() {
    this.identityService.signedIn$.subscribe(signedIn => {
      if (signedIn) {
        this.addLocalCartItemsToServer();
      } else {
        this.cartItemThumbnails = [];
        this.cartItemThumbnailsSource.next(this.cartItemThumbnails);
      }
    });
    this.updateCartItemThumbnails();
  }

  public updateCartItemThumbnails(): void {
    if (this.identityService.signedIn) {
      this.loadFromServer();
    } else {
      this.loadFromLocal();
    }
  }

  public addToCart(cartItemToAdd: CartItem): Observable<CartItemThumbnail[]> {
    let cartItemSubject = new Subject<CartItemThumbnail[]>();
    if (this.identityService.signedIn) {
      this.putCustom<CartItemThumbnail>(cartItemToAdd, this.getUrlWithParameter('addToCart'), {}, this.postAuthenticationRequired, this.defaultHttpHeaders)
        .subscribe((added: CartItemThumbnail) => {
          if (added) {
            this.updateCartItem(new CartItemThumbnail(added));
            cartItemSubject.next(this.cartItemThumbnails);
          }
        });
    } else {
      let existing = this.cartItemThumbnails.find(c => c.itemVariant.id == cartItemToAdd.itemVariantId);
      if (existing) {
        existing.number += cartItemToAdd.number;
        this.updateCartItem(existing);
        this.updateCartLocal(existing);
      } else {
        this.itemVariantService.get(cartItemToAdd.itemVariantId)
          .subscribe((variant: ItemVariant) => {
            this.itemService.get(variant.itemId)
              .subscribe((item: Item) => {
                let newCartItem = new CartItemThumbnail({ item: item, itemVariant: variant, number: cartItemToAdd.number });
                this.updateCartItem(newCartItem);
                this.updateCartLocal(newCartItem);
              });
          })
      }
    };
    cartItemSubject.next(this.cartItemThumbnails);
    return cartItemSubject.asObservable();
  }

  public removeFromCart(cartItem: CartItem): Observable<CartItemThumbnail[]> {
    let cartItemSubject = new Subject<CartItemThumbnail[]>();
    let existing = this.cartItemThumbnails.find(c => c.itemVariant.id == cartItem.itemVariantId);
    if (existing) {
      existing.number -= cartItem.number;
      if (this.identityService.signedIn) {
        this.putCustom<Response>(cartItem, this.getUrlWithParameter('removeFromCart'), {}, this.postAuthenticationRequired, this.defaultHttpHeaders)
          .subscribe((response: Response) => {
            if (response) {
              this.updateCartItem(existing);
            }
          });
      } else {
        this.updateCartLocal(existing);
        this.updateCartItem(existing);
      }
    }
    cartItemSubject.next(this.cartItemThumbnails);
    return cartItemSubject.asObservable();
  }

  protected updateCartItem(cartItemThumbnail: CartItemThumbnail): CartItemThumbnail {
    if (cartItemThumbnail.number <= 0) {
      this.cartItemThumbnails = this.cartItemThumbnails.filter(c => c.itemVariant.id != cartItemThumbnail.itemVariant.id);
    } else {
      let index = this.cartItemThumbnails.findIndex(c => c.itemVariant.id == cartItemThumbnail.itemVariant.id);
      if (index >= 0) {
        this.cartItemThumbnails[index] = cartItemThumbnail;
      }
      else {
        this.cartItemThumbnails.push(cartItemThumbnail);
      }
    }
    this.cartItemThumbnailsSource.next(this.cartItemThumbnails);
    return cartItemThumbnail;
  }

  public indexThumbnails() {
    return this.getCustom<EntityIndex<CartItemThumbnail>>(this.getUrlWithParameter(ParameterNames.thumbnails), {}, this.defaultHttpHeaders, this.getAuthenticationRequired);
  }

  protected loadFromLocal() {
    let items = this.readLocalCartItems();
    items.forEach(i => this.addToCart(i));
    this.cartItemThumbnailsSource.next(this.cartItemThumbnails);
  }

  protected loadFromServer() {
    this.indexThumbnails()
      .subscribe((index: EntityIndex<CartItemThumbnail>) => {
        if (index) {
          index.entities.forEach(i => {
            this.updateCartItem(new CartItemThumbnail(i));
          });
        }
      });
    this.cartItemThumbnailsSource.next(this.cartItemThumbnails);
  }

  protected updateCartLocal(cartItemThumbnail: CartItemThumbnail): void {
    let localItems = this.readLocalCartItems();
    let index = localItems.findIndex(c => c.itemVariantId == cartItemThumbnail.itemVariant.id);
    if (cartItemThumbnail.number > 0) {
      if (index < 0) {
        index = localItems.push(new CartItem({ itemVariantId: cartItemThumbnail.itemVariant.id })) - 1;
      }
      localItems[index].number = cartItemThumbnail.number;
    } else {
      if (index >= 0) {
        localItems = localItems.filter(i => i != localItems[index]);
      }
    }
    localStorage.setItem(this.localStorageKey, JSON.stringify(localItems));
  }

  protected readLocalCartItems(): CartItem[] {
    let result: CartItem[] = [];
    let saved = localStorage.getItem(this.localStorageKey);
    if (saved) {
      result = JSON.parse(saved);
    }
    return result;
  }

  protected delay(ms: number): Promise<{}> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  protected addLocalCartItemsToServer() {
    let oldCartItemThumbnails = this.cartItemThumbnails.slice();
    this.cartItemThumbnails = [];
    oldCartItemThumbnails.forEach(c => {
      this.addToCart(new CartItem({ itemVariantId: c.itemVariant.id, number: c.number }));
    });
    localStorage.removeItem(this.localStorageKey);
  }
}
