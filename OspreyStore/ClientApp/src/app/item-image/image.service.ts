import { HttpClient } from '@angular/common/http';
import { FileInfoService } from './file-info.service';
import { Router } from '@angular/router';
import { Image } from './image';
import { Entity } from '../models/entity/entity';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';

export abstract class ImageService<TEntity extends Entity> extends FileInfoService<Image<TEntity>> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.serviceName = 'Image service';
  }
}
