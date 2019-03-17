import { Component, OnInit } from '@angular/core';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { OrderService } from '../order.service';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { OrderThumbnail } from '../order-thumbnail';
import { OrderStatus, Order } from '../order';
import { IdentityService } from 'src/app/identity/identity.service';
import { MessageService } from 'src/app/message/message.service';

@Component({
  selector: 'app-order-thumbnail-index',
  templateUrl: './order-thumbnail-index.component.html',
  styleUrls: ['./order-thumbnail-index.component.css']
})
export class OrderThumbnailIndexComponent implements OnInit {
  orderIndex: EntityIndex<OrderThumbnail>;
  private _currentFilter: Filter;
  public get currentFilter(): Filter {
    return this._currentFilter;
  }
  public set currentFilter(f: Filter) {
    this._currentFilter = f;
    this.getOrders();
  }
  filterValues: Filter[];
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
    this.filterValues = [];
    let storeIds = this.identityService.getUserAdministredStoreIds();
    if (storeIds.length > 0) {
      this.userStoreId = storeIds[0];
      this.filterValues.push(Filter.MyStore);
      this.isStoreAdministrator = this.identityService.isStoreAdministrator(this.userStoreId);
    }
    this.filterValues.push(Filter.MyOwn);
    this.currentFilter = this.filterValues[0];
    this.getOrders();
  }

  getOrders() {
    let storeId = 0;
    if (this.currentFilter == Filter.MyStore) {
      storeId = this.userStoreId;
    }
    this.orderService.indexThumbnails(storeId).subscribe((data) => {
      this.orderIndex = data;
    });
  }

  cancelOrder(order: Order) {
    order.status = OrderStatus.Cancelled;
    this.orderService.put(order).subscribe((data) => {
      this.messageService.sendSuccess("Order cancelled");
    });
  }

  finishOrder(order: Order) {
    order.status = OrderStatus.Finished;
    this.orderService.put(order).subscribe((data) => {
      this.messageService.sendSuccess("Order finished");
    });
  }

  shouldShowActionButtons(order: Order) {
    return this.isStoreAdministrator && this.currentFilter == Filter.MyStore && order.status == OrderStatus.Created;
  }

  getStatusString(status: OrderStatus): string {
    return OrderStatus[status];
  }

  getFilterString(filter: Filter): string {
    return Filter[filter];
  }

}

export enum Filter {
  MyOwn,
  MyStore
}
