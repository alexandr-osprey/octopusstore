import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ImageService } from './image.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';
import { ItemVariantImage } from './item-variant-image';
import { ItemVariant } from '../item-variant/item-variant';

@Injectable({
  providedIn: 'root'
})
export class ItemVariantImageService extends ImageService<ItemVariant, ItemVariantImage> {
  public postFilePrefix = '/api/itemVariants/';
  public postFileSuffix = 'image';

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/itemVariantImages';
    this.serviceName = 'Item Image service';
  }

  public getImageUrl(id: number): string {
    return this.getUrlWithIdWithSuffix(id, 'file');
  }
  public postFile(body: any, id: number, params: any = {}): Observable<ItemVariantImage> {
    return this.post(body, this.getUrlWithIdWithSuffix(id, this.postFileSuffix, this.postFilePrefix), params, new HttpHeaders())
  }
}
