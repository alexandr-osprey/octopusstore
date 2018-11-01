import { Injectable } from '@angular/core';
import { DataReadWriteService } from './data-read-write-service';
import { ItemVariantIndex } from '../view-models/item-variant/item-variant-index';
import { ItemVariantDetail } from '../view-models/item-variant/item-variant-detail';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { ItemVariant } from '../view-models/item-variant/item-variant';
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ItemVariantService extends DataReadWriteService<
  ItemVariant,
  ItemVariantIndex,
  ItemVariantDetail> {

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
