import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Order } from '../order';
import { MessageService } from 'src/app/message/message.service';
import { OrderService } from '../order.service';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import 'rxjs/add/observable/zip';
import { Observable } from 'rxjs';
import { forEach } from '@angular/router/src/utils/collection';
import { CartItem } from 'src/app/cart/cart-item/cart-item';
import { CartItemService } from 'src/app/cart/cart-item/cart-item.service';

@Component({
  selector: 'app-order-create',
  templateUrl: './order-create.component.html',
  styleUrls: ['./order-create.component.css']
})
export class OrderCreateComponent implements OnInit {
  order: Order;
  isUpdating: boolean = false;
  cartItems: CartItem[];
  @Output() orderSaved = new EventEmitter<Order>();

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private cartItemService: CartItemService,
    private messageService: MessageService,
    private router: Router,) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id != 0)
      this.isUpdating = true;
    this.cartItemService.index().subscribe((data: EntityIndex<CartItem>) => {
      if (data != null) {
        this.cartItems = data.entities;
        let observables: Observable<any>;
        observables = Observable.zip(...this.cartItems.map(i =>
          this.orderService.post(new Order({ itemVariantId: i.itemVariantId, number: i.number }))
        ));
        observables.subscribe((data) => {
          if (data != null) {
            this.router.navigate(['/orders']);
          }
        });
      }
    });
  }
}
