import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Order } from '../order';
import { MessageService } from 'src/app/message/message.service';
import { OrderService } from '../order-service.service';
import { CartItemService } from 'src/app/cart-item/cart-item.service';

@Component({
  selector: 'app-order-create-update',
  templateUrl: './order-create-update.component.html',
  styleUrls: ['./order-create-update.component.css']
})
export class OrderCreateUpdateComponent implements OnInit {
  order: Order;
  @Output() orderSaved = new EventEmitter<Order>();

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private cartItemService: CartItemService,
    private messageService: MessageService,
    private location: Location) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id == 0)
      throw new Error("Wrong order");
    //this.cartItemService.index().subscribe();
  }
}
