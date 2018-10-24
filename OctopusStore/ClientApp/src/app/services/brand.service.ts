import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BrandIndex } from '../view-models/brand/brand-index';
import { MessageService } from './message.service';
import { BrandDetails } from '../view-models/brand/brand-details';
import { Brand } from '../view-models/brand/brand';
import { IdentityService } from './identity-service';
import { DataReadWriteService } from './data-read-write-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class BrandService extends DataReadWriteService<Brand, BrandIndex, BrandDetails> {

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
