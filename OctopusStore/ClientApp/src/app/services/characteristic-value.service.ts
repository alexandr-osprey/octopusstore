import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataReadService } from './data-read-service';
import { MessageService } from './message.service';
import { CharacteristicValueIndex } from '../view-models/characteristic-value/characteristic-value-index';
import { CharacteristicValueDetail } from '../view-models/characteristic-value/characteristic-value-detail';
import { CharacteristicValue } from '../view-models/characteristic-value/characteristic-value';

@Injectable({
  providedIn: 'root'
})
export class CharacteristicValueService extends DataReadService<CharacteristicValue, CharacteristicValueIndex, CharacteristicValueDetail> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/characteristicValues';
    this.serviceName = 'Characteristic values service';
  }
}
