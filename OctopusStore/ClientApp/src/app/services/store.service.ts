import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { StoreIndex } from '../view-models/store/store-index';
import { StoreDetails } from '../view-models/store/store-details';
import { Store } from '../view-models/store/store';
import { DataReadWriteService } from './data-read-write-service';
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Index } from '../view-models';

@Injectable({
  providedIn: 'root',
})
export class StoreService extends DataReadWriteService<Store, StoreIndex, StoreDetails> {
  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/stores';
    this.serviceName = 'Store service';
  }

  public indexStoreAdministrators(storeId: number): Observable<Index<string>> {
    let url = this.getUrlWithIdWithSuffix(storeId, "administrators");
    return this.getCustom(url, {}, this.defaultHttpHeaders, true);
  }
  public postStoreAdministrator(storeId: number, storeAdministrator: string): Observable<string> {
    let url = this.getUrlWithIdWithSuffix(storeId, "administrators");
    let headers = this.defaultHttpHeaders.append("email", storeAdministrator);
    return this.postCustom(storeAdministrator, url, {}, true, headers);
  }
  public deleteStoreAdministrator(storeId: number, storeAdministrator: string): Observable<string> {
    let url = this.getUrlWithIdWithSuffix(storeId, "administrators");
    let headers = this.defaultHttpHeaders.append("email", storeAdministrator);
    return this.deleteCustom(url, {}, headers, true);
  }
}
