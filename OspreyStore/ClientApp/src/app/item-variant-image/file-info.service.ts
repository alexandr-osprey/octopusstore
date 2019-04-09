import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DataReadWriteService } from '../services/data-read-write.service';
import { Entity } from '../models/entity/entity';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';

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
