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

  public indexStoreAdministrators(storeId: number): Observable<Index<string>> {
    let url = this.getUrlWithIdWithSuffix(storeId, "administrators");
    return this.getCustom(url, {}, this.defaultHttpHeaders);
  }
  public postStoreAdministrator(storeId: number, storeAdministrator: string): Observable<string> {
    let url = this.getUrlWithIdWithSuffix(storeId, "administrators");
    let headers = this.defaultHttpHeaders.append("email", storeAdministrator);
    return this.postCustom(storeAdministrator, url, {}, headers);
  }
  public deleteStoreAdministrator(storeId: number, storeAdministrator: string): Observable<string> {
    let url = this.getUrlWithIdWithSuffix(storeId, "administrators");
    let headers = this.defaultHttpHeaders.append("email", storeAdministrator);
    return this.deleteCustom(url, {}, headers, true);
  }
}
