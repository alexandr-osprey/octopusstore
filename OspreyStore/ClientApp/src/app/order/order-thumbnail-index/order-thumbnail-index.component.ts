import { Component, OnInit } from '@angular/core';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { OrderService } from '../order.service';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { OrderThumbnail } from '../order-thumbnail';
import { OrderStatus, Order } from '../order';
import { IdentityService } from 'src/app/identity/identity.service';
import { MessageService } from 'src/app/message/message.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';

@Component({
  selector: 'app-order-thumbnail-index',
  templateUrl: './order-thumbnail-index.component.html',
  styleUrls: ['./order-thumbnail-index.component.css']
})
export class OrderThumbnailIndexComponent implements OnInit {
  orderIndex: EntityIndex<OrderThumbnail>;
  private isStoreView = false;
  userStoreId: number;
  isStoreAdministrator: boolean;

  constructor(
    private orderService: OrderService,
    private identityService: IdentityService,
    private parameterService: ParameterService,
    private messageService: MessageService,
  ) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let storeIds = this.identityService.getUserAdministredStoreIds();
    this.isStoreView = this.parameterService.getParam(ParameterNames.storeId) as boolean;
    if (storeIds.length > 0) {
      this.userStoreId = storeIds[0];
      this.isStoreAdministrator = this.identityService.isStoreAdministrator(this.userStoreId);
    }
    this.getOrders();
  }

  getOrders() {
    let storeId = 0;
    if (this.isStoreView) {
      storeId = this.userStoreId;
    }
    this.orderService.indexThumbnails(storeId).subscribe((data) => {
      this.orderIndex = data;
    });
  }

  cancelOrder(order: Order) {
    order.status = OrderStatus.Cancelled;
    this.orderService.put(order).subscribe((data) => {
      if (data) {
        this.messageService.sendSuccess("Order cancelled");
      }
    });
  }

  finishOrder(order: Order) {
    order.status = OrderStatus.Finished;
    this.orderService.put(order).subscribe((data) => {
      if (data) {
        this.messageService.sendSuccess("Order finished");
      }
    });
  }

  shouldShowCancelButton(order: Order) {
    return (this.isStoreView && this.isStoreAdministrator && order.status == OrderStatus.Created)
      || (!this.isStoreView && order.status == OrderStatus.Created);
  }

  shouldShowFinishButton(order: Order) {
    return (this.isStoreView && this.isStoreAdministrator && order.status == OrderStatus.Created);
  }

  getStatusString(status: OrderStatus): string {
    return OrderStatus[status];
  }

  setView(isStoreView: boolean) {
    this.isStoreView = isStoreView;
    this.getOrders();
  }
}
