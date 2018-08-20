import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataReadService } from './data-read-service';
import { BrandIndex } from '../view-models/brand/brand-index';
import { MessageService } from './message.service';
import { BrandDetails } from '../view-models/brand/brand-details';
import { Brand } from '../view-models/brand/brand';

@Injectable({
  providedIn: 'root'
})
export class BrandService extends DataReadService<Brand, BrandIndex, BrandDetails> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/brands';
    this.serviceName = 'Brand service';
  }
}
