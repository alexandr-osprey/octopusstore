import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { StoreIndex } from '../view-models/store/store-index';
import { StoreDetail } from '../view-models/store/store-detail';
import { Store } from '../view-models/store/store';
import { DataReadWriteService } from './data-read-write-service';

@Injectable({
  providedIn: 'root'
})
export class StoreService extends DataReadWriteService<Store, StoreIndex, StoreDetail> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/stores';
    this.serviceName = 'Store service';
  }
}
