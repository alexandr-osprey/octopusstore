import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ItemProperty } from './item-property';
import { DataReadWriteService } from '../services/data-read-write.service';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';

@Injectable({
  providedIn: 'root'
})
export class ItemPropertyService extends DataReadWriteService<ItemProperty> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/itemProperties';
    this.serviceName = 'Item properties service';
  }
}
