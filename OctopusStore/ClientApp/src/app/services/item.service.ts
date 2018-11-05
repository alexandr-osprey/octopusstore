import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service'
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ParameterService } from './parameter-service';
import { ParameterNames } from './parameter-names';
import { DataReadWriteService } from './data-read-write-service';
import { Item } from '../view-models/item';
import { EntityIndex } from '../view-models/entity-index';
import { ItemThumbnail } from '../view-models/item-thumbnail';


@Injectable()
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
      this.defaultHttpHeaders,
      this.getAuthenticationRequired
      );
  }
}
