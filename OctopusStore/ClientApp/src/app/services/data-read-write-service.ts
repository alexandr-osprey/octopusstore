import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map, tap, retry } from 'rxjs/operators';
import { DataReadService } from './data-read-service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MessageService } from './message.service';
import { Entity } from '../view-models/entity';
import { EntityDetail } from '../view-models/entity-detail';
import { EntityIndex } from '../view-models/entity-index';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

export abstract class DataReadWriteService<
  TEntity extends Entity,
  TIndex extends EntityIndex<TEntity>,
  TDetail extends EntityDetail<TEntity>>
  extends DataReadService<TEntity, TIndex, TDetail> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService)
  }

  public createOrUpdate(model: TEntity, url: string = this.remoteUrl): Observable<TEntity> {
    if (model.id == 0)
      return this.create(model, url);
    else
      return this.update(model, url);
  }
  public create(model: TEntity, url: string = this.remoteUrl): Observable<TEntity> {
    return this.http.post<TEntity>(url, model, httpOptions)
      .pipe(
        tap(entity => this.log(`${this.serviceName} created id= ${entity.id}`)),
      catchError(this.handleError<any>(`create error ` + model)));
  }
  public postFormData(formFile: FormData, url: string): Observable<TEntity> {
    return this.http.post<TEntity>(url, formFile)
      .pipe(
        tap(entity => this.log(`${this.serviceName} created id= ${entity.id}`)),
        catchError(this.handleError<any>(`create error `)));
  }

  public update(model: TEntity, url: string = this.remoteUrl): Observable<TEntity> {
    return this.http.put<TEntity>(this.getUrlWithId(model.id, url), model, this.httpOptions)
      .pipe(
      tap(entity => { this.log(`${this.serviceName} updated id=${entity.id}`) }),
        catchError(this.handleError<any>(`update error id=${model.id}`)));
  }

  public delete(id: number): Observable<any> {
    return this.http.delete<any>(this.getUrlWithId(id), this.httpOptions)
      .pipe(
        tap(_ => this.log(`${this.serviceName} deleted id=${id}`)),
        catchError(this.handleError<any>(`delete error id=${id}`)));
  }
}
