import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service'
import { ItemDetails } from '../view-models/item/item-details';
import { ItemIndex } from '../view-models/item/item-index';
import { DataReadWriteService } from './data-read-write-service';
import { Item } from '../view-models/item/item';
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ParameterService } from './parameter-service';
import { ItemThumbnailIndex } from '../view-models/item/item-thumbnail-index';
import { ParameterNames } from './parameter-names';


@Injectable()
export class ItemService extends DataReadWriteService<Item, ItemIndex, ItemDetails> {
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

  public indexItemThumbnails(): Observable<ItemThumbnailIndex> {
    return this.getCustom<ItemThumbnailIndex>(
      this.getUrlWithParameter(ParameterNames.thumbnails),
      this.parameterService.getParams(),
      this.defaultHttpHeaders,
      this.getAuthenticationRequired
      );
  }
}
