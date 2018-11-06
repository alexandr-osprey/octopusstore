import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { DataReadWriteService } from './data-read-write.service';
import { IdentityService } from './identity.service';
import { Router } from '@angular/router';
import { ItemProperty } from '../view-models/item-property/item-property';

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
