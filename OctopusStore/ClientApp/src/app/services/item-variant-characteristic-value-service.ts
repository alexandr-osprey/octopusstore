import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { ItemVariantCharacteristicValueDetail } from '../view-models/item-variant-characteristic-value/item-variant-characteristic-value-detail';
import { ItemVariantCharacteristicValueIndex } from '../view-models/item-variant-characteristic-value/item-variant-characteristic-value-index';
import { ItemVariantCharacteristicValue } from '../view-models/item-variant-characteristic-value/item-variant-characteristic-value';
import { DataReadWriteService } from './data-read-write-service';

@Injectable({
  providedIn: 'root'
})
export class ItemVariantCharacteristicValueService extends DataReadWriteService<
  ItemVariantCharacteristicValue,
  ItemVariantCharacteristicValueIndex,
  ItemVariantCharacteristicValueDetail> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {

    super(http, messageService);
    this.remoteUrl = 'api/itemVariantCharacteristicValues';
    this.serviceName = 'Item variant characteristic service';
  }
}
