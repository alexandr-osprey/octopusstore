import { Injectable } from '@angular/core';
import { DataReadWriteService } from './data-read-write-service';
import { ItemVariantIndex } from '../view-models/item-variant/item-variant-index';
import { ItemVariantDetails } from '../view-models/item-variant/item-variant-details';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { Observable } from 'rxjs';
import { ItemVariant } from '../view-models/item-variant/item-variant';

@Injectable({
  providedIn: 'root'
})
export class ItemVariantService extends DataReadWriteService<
  ItemVariant,
  ItemVariantIndex,
  ItemVariantDetails> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/itemVariants';
    this.serviceName = 'Item variant service';
  }
}
