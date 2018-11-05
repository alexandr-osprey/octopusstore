import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { Brand } from '../view-models/brand';
import { IdentityService } from './identity-service';
import { DataReadWriteService } from './data-read-write-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class BrandService extends DataReadWriteService<Brand> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/brands';
    this.serviceName = 'Brand service';
  }
}
