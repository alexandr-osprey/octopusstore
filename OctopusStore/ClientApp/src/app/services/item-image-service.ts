import { Injectable } from '@angular/core';
import { Image } from '../view-models/image/image';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ImageService } from './image.service';
import { Item } from '../view-models/item/item';
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ItemImage } from '../view-models/item-image/item-image';

@Injectable({
  providedIn: 'root'
})
export class ItemImageService extends ImageService<Item, Image<Item>> {
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
