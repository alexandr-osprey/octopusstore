import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DataReadWriteService } from '../services/data-read-write.service';
import { ItemVariant } from './item-variant';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';

@Injectable({
  providedIn: 'root'
})
export class ItemVariantService extends DataReadWriteService<ItemVariant> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/itemVariants';
    this.serviceName = 'Item variant service';
  }
}
