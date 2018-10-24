import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { CharacteristicValueIndex } from '../view-models/characteristic-value/characteristic-value-index';
import { CharacteristicValueDetails } from '../view-models/characteristic-value/characteristic-value-details';
import { CharacteristicValue } from '../view-models/characteristic-value/characteristic-value';
import { IdentityService } from './identity-service';
import { DataReadWriteService } from './data-read-write-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CharacteristicValueService extends DataReadWriteService<CharacteristicValue, CharacteristicValueIndex, CharacteristicValueDetails> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/characteristicValues';
    this.serviceName = 'Characteristic value service';
  }
}
