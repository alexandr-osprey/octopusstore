import { Injectable } from '@angular/core';
import { Entity } from '../view-models/entity';
import { DataReadWriteService } from './data-read-write-service';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { FileInfoIndex } from '../view-models/file-info/file-info-index';
import { FileInfoDetail } from '../view-models/file-info/file-info-detail';
import { IdentityService } from './identity-service';
import { Router } from '@angular/router';

export abstract class FileInfoService<
  TEntity extends Entity,
  TIndex extends FileInfoIndex<TEntity>,
  TDetail extends FileInfoDetail<TEntity>> extends DataReadWriteService<
  TEntity,
  TIndex,
  TDetail> {

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
