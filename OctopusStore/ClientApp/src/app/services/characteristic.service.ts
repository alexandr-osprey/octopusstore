import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { CharacteristicIndex } from '../view-models/characteristic/characteristic-index';
import { CharacteristicDetails } from '../view-models/characteristic/characteristic-details';
import { Characteristic } from '../view-models/characteristic/characteristic';
import { IdentityService } from './identity-service';
import { DataReadWriteService } from './data-read-write-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CharacteristicService extends DataReadWriteService<Characteristic, CharacteristicIndex, CharacteristicDetails> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/characteristics';
    this.serviceName = 'Characteristic service';
  }
}
