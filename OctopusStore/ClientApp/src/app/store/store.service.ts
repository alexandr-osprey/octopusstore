import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DataReadWriteService } from '../services/data-read-write.service';
import { Store } from './store';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';
import { Index } from '../models';

@Injectable({
  providedIn: 'root',
})
export class StoreService extends DataReadWriteService<Store> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/stores';
    this.serviceName = 'Store service';
  }
}
