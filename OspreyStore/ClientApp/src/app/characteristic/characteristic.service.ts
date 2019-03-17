import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DataReadWriteService } from '../services/data-read-write.service';
import { Characteristic } from './characteristic';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';

@Injectable({
  providedIn: 'root'
})
export class CharacteristicService extends DataReadWriteService<Characteristic> {

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
