import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DataReadWriteService } from '../services/data-read-write.service';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';
import { Order } from './order';

@Injectable({
  providedIn: 'root'
})
export class OrderService extends DataReadWriteService<Order> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/orders';
    this.serviceName = 'Orders service';
  }
}
