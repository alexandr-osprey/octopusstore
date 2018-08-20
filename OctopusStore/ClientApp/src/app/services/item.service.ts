import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MessageService } from './message.service'
import { forEach } from '@angular/router/src/utils/collection';
import { ItemDetails } from '../view-models/item/item-details';
import { ItemIndex } from '../view-models/item/item-index';
import { DataReadWriteService } from './data-read-write-service';
import { retry, tap, catchError } from 'rxjs/operators';
import { Item } from '../view-models/item/item';


@Injectable()
export class ItemService extends DataReadWriteService<Item, ItemIndex, ItemDetails> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/items';
    this.serviceName = 'Item service';
  }
}
