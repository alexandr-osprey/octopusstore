import { Component, OnInit } from '@angular/core';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { OrderService } from '../order.service';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { OrderThumbnail } from '../order-thumbnail';
import { OrderStatus } from '../order';

@Component({
  selector: 'app-order-thumbnail-index',
  templateUrl: './order-thumbnail-index.component.html',
  styleUrls: ['./order-thumbnail-index.component.css']
})
export class OrderThumbnailIndexComponent implements OnInit {
  orderIndex: EntityIndex<OrderThumbnail>;

  constructor(
    private orderService: OrderService,
    private parameterService: ParameterService,
  ) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.orderService.indexThumbnails().subscribe((data) => {
      this.orderIndex = data;
    });
  }

  getStatusString(status: OrderStatus): string {
    return OrderStatus[status];
  }
}
