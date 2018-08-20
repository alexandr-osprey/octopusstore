import { Injectable } from '@angular/core';
import { Image } from '../view-models/image/image';
import { Entity } from '../view-models/entity';
import { DataReadWriteService } from './data-read-write-service';
import { ImageIndex } from '../view-models/image/image-index';
import { ImageDetail } from '../view-models/image/image-detail';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { ImageService } from './image.service';
import { Item } from '../view-models/item/item';
import { ParameterNames } from './parameter-names';

@Injectable({
  providedIn: 'root'
})
export class ItemImageService extends ImageService<Item, Image<Item>> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/itemImages/';
    this.filePostUrl = this.remoteUrl + ParameterNames.file;
    this.serviceName = 'Item Image service';
  }
  public getImageUrl(id: number): string {
    return this.getUrlWithId(id) + '/file/';
  }
}
