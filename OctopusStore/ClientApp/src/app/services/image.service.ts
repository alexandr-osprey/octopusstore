import { Image } from '../view-models/image/image';
import { Entity } from '../view-models/entity/entity';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { FileInfoService } from './file-info.service';
import { IdentityService } from './identity.service';
import { Router } from '@angular/router';

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
