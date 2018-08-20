import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { ItemVariantCharacteristicValueIndex } from '../view-models/item-variant-characteristic-value/item-variant-characteristic-value-index';
import { ItemVariantCharacteristicValue } from '../view-models/item-variant-characteristic-value/item-variant-characteristic-value';
import { DataReadWriteService } from './data-read-write-service';
import { ItemVariantCharacteristicValueDetails } from '../view-models/item-variant-characteristic-value/item-variant-characteristic-value-details';

@Injectable({
  providedIn: 'root'
})
export class ItemVariantCharacteristicValueService extends DataReadWriteService<
  ItemVariantCharacteristicValue,
  ItemVariantCharacteristicValueIndex,
  ItemVariantCharacteristicValueDetails> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {

    super(http, messageService);
    this.remoteUrl = 'api/itemVariantCharacteristicValues';
    this.serviceName = 'Item variant characteristic service';
  }
}
