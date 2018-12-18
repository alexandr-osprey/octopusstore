import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DataReadWriteService } from '../services/data-read-write.service';
import { Item } from './item';
import { IdentityService } from '../identity/identity.service';
import { ParameterService } from '../parameter/parameter.service';
import { MessageService } from '../message/message.service';
import { EntityIndex } from '../models/entity/entity-index';
import { ItemThumbnail } from './item-thumbnail';
import { ParameterNames } from '../parameter/parameter-names';


@Injectable({
  providedIn: 'root'
})
export class ItemService extends DataReadWriteService<Item> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    private parameterService: ParameterService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/items';
    this.serviceName = 'Item service';
    this.getAuthenticationRequired = true;
  }

  public indexItemThumbnails(): Observable<EntityIndex<ItemThumbnail>> {
    return this.getCustom<EntityIndex<ItemThumbnail>>(
      this.getUrlWithParameter(ParameterNames.thumbnails),
      this.parameterService.getParams(),
      this.defaultHttpHeaders
      );
  }
}
