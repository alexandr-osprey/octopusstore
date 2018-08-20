import { Injectable } from '@angular/core';
import { Entity } from '../view-models/entity';
import { DataReadWriteService } from './data-read-write-service';
import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { FileDetailsIndex } from '../view-models/file-details/file-details-index';
import { FileDetailsDetails } from '../view-models/file-details/file-details-details';
import { ParameterNames } from './parameter-names';

export abstract class FileDetailsService<
  TEntity extends Entity,
  TIndex extends FileDetailsIndex<TEntity>,
  TDetails extends FileDetailsDetails<TEntity>> extends DataReadWriteService<
  TEntity,
  TIndex,
  TDetails> {

  public filePostUrl: string;

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.serviceName = 'File details service';
  }
}
