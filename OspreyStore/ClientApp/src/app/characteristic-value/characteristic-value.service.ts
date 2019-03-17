import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DataReadWriteService } from '../services/data-read-write.service';
import { CharacteristicValue } from './characteristic-value';
import { MessageService } from '../message/message.service';
import { IdentityService } from '../identity/identity.service';

@Injectable({
  providedIn: 'root'
})
export class CharacteristicValueService extends DataReadWriteService<CharacteristicValue> {

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
