import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map, tap, retry } from 'rxjs/operators';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Entity } from '../view-models/entity';
import { EntityIndex } from '../view-models/entity-index';
import { EntityDetail } from '../view-models/entity-detail';

export abstract class DataReadService
  <TEntity extends Entity,
  TIndex extends EntityIndex<TEntity>,
  TDetail extends EntityDetail<TEntity>> {
  protected remoteUrl: string;
  protected serviceName: string = "Data service";
  protected httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
  }

  public get(id: number, params: any = {}): Observable<TEntity> {
    return this.http.get<TEntity>(this.getUrlWithId(id), { params: params })
      .pipe(
        retry(3),
        tap(_ => this.log(`${this.serviceName} fetched id=${id}`)),
      catchError(this.handleError<TEntity>(`entity id=${id}`)));
  }
  public getDetail(id: number, params: any = {}): Observable<TDetail> {
    return this.http.get<TDetail>(this.getUrlWithIdWithSuffix(id, "details"), { params: params })
      .pipe(
        retry(3),
        tap(_ => this.log(`${this.serviceName} fetched details id=${id}`)),
        catchError(this.handleError<TDetail>(`details id=${id}`)));
  }
  public index(params: any = {}): Observable<TIndex> {
    return this.indexCustom<TIndex>(this.remoteUrl, params);
  }
  public indexCustom<T>(url: string, params: any = {}): Observable<T> {
    return this.http.get<T>(url, { params: params })
      .pipe(
        retry(3),
        tap(entities => this.log(this.serviceName + ' fetched index')),
        catchError(this.handleError<T>('index')));
  }
  public getUrlWithId(id: number, url: string = this.remoteUrl) {
    return this.getUrlWithParameter(`${id}`, url);
  }
  public getUrlWithIdWithSuffix(id: number, suffix: string, url: string = this.remoteUrl) {
    return `${url}/${id}/${suffix}`;
  }
  public getUrlWithParameter(parameter: string, url: string = this.remoteUrl) {
    return `${url}/${parameter}`;
  }

  protected handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  protected log(message: string) {
    this.messageService.add(this.serviceName + ' ' + message);
    console.log(message);
  }
}
