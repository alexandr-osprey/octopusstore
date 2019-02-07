import { Component, OnInit } from '@angular/core';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { Order } from '../order';
import { OrderService } from '../order.service';
import { ParameterService } from 'src/app/parameter/parameter.service';

@Component({
  selector: 'app-order-index',
  templateUrl: './order-index.component.html',
  styleUrls: ['./order-index.component.css']
})
export class OrderIndexComponent implements OnInit {
  orderIndex: EntityIndex<Order>;

  constructor(
    private orderService: OrderService,
    private parameterService: ParameterService,
  ) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.orderService.index().subscribe((data) => {
      this.orderIndex = data;
    });
  }

}
