import { Entity } from '../view-models/entity';
import { DataReadWriteService } from './data-read-write-service';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { IdentityService } from './identity-service';

export abstract class FileInfoService<TEntity extends Entity> extends DataReadWriteService<TEntity> {
  public filePostUrl: string;

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.serviceName = 'File detail service';
  }
}
