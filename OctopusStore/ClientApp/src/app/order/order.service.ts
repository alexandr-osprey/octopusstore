import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DataReadWriteService } from '../services/data-read-write.service';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';
import { Order } from './order';
import { Observable } from 'rxjs';
import { EntityIndex } from '../models/entity/entity-index';
import { OrderThumbnail } from './order-thumbnail';
import { ParameterNames } from '../parameter/parameter-names';
import { ParameterService } from '../parameter/parameter.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService extends DataReadWriteService<Order> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    private parameterService: ParameterService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/orders';
    this.serviceName = 'Orders service';
  }

  public indexThumbnails(): Observable<EntityIndex<OrderThumbnail>> {
    // TO DO implement normal filtering!
    this.parameterService.navigateWithUpdatedParams(["ownerId", this.identityService.currentUserEmail]);
    return this.getCustom<EntityIndex<OrderThumbnail>>(
      this.getUrlWithParameter(ParameterNames.thumbnails),
      this.parameterService.getParams(),
      this.defaultHttpHeaders);
  }
}
