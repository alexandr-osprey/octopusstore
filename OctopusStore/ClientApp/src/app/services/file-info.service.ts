import { Injectable } from '@angular/core';
import { Entity } from '../view-models/entity';
import { DataReadWriteService } from './data-read-write-service';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { FileInfoIndex } from '../view-models/file-info/file-info-index';
import { FileInfoDetails } from '../view-models/file-info/file-info-details';
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';

export abstract class FileInfoService<
  TEntity extends Entity,
  TIndex extends FileInfoIndex<TEntity>,
  TDetails extends FileInfoDetails<TEntity>> extends DataReadWriteService<
  TEntity,
  TIndex,
  TDetails> {

  public filePostUrl: string;

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.serviceName = 'File details service';
  }
}
