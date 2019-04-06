import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ImageService } from './image.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Item } from '../item/item';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';
import { ItemImage } from './item-image';

@Injectable({
  providedIn: 'root'
})
export class ItemImageService extends ImageService<Item, ItemImage> {
  public postFilePrefix = '/api/items/';
  public postFileSuffix = 'itemImage';

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/itemImages/';
    this.serviceName = 'Item Image service';
  }

  public getImageUrl(id: number): string {
    return this.getUrlWithIdWithSuffix(id, '/file/');
  }
  public postFile(body: any, id: number, params: any = {}): Observable<ItemImage> {
    return this.post(body, this.getUrlWithIdWithSuffix(id, this.postFileSuffix, this.postFilePrefix), params, new HttpHeaders())
  }
}
