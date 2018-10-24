import { Injectable } from '@angular/core';
import { Image } from '../view-models/image/image';
import { Entity } from '../view-models/entity';
import { ImageIndex } from '../view-models/image/image-index';
import { ImageDetails } from '../view-models/image/image-detail';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { FileInfoService } from './file-info.service';
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';

export abstract class ImageService<
  TEntity extends Entity, TImage extends
  Image<TEntity>> extends FileInfoService<Image<TEntity>,
  ImageIndex<Image<TEntity>>,
  ImageDetails<Image<TEntity>>> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.serviceName = 'Image service';
  }
}
