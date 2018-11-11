import { Observable, of, Subject } from 'rxjs';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders, HttpHeaderResponse } from '@angular/common/http';
import { Entity } from '../view-models/entity/entity';
import { IdentityService } from './identity.service';
import 'rxjs/add/operator/retryWhen';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/delay';
import 'rxjs/add/operator/take';
import { Router } from '@angular/router';
import { ParameterNames } from './parameter-names';
import { EntityIndex } from '../view-models/entity/entity-index';

export abstract class DataReadWriteService <TEntity extends Entity> {
  protected remoteUrl: string;
  protected serviceName: string = "Data service";
  protected get defaultHttpHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Content-Type': 'application/json' });
  }
  protected retryLimit: number = 3;
  protected delayMilliseconds = 2 * 1000;

  protected postAuthenticationRequired: boolean = true;
  protected getAuthenticationRequired: boolean = false;
  protected putAuthenticationRequired: boolean = true;
  protected deleteAuthenticationRequired: boolean = true;

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
  }
  public post(body: any, url: string = this.remoteUrl, params: any = {}, headers: HttpHeaders = this.defaultHttpHeaders): Observable<TEntity> {
    return this.postCustom(body, url, params, this.postAuthenticationRequired, headers);
  }
  public postCustom<T>(body: any, url: string, params: any, authenticationRequired: boolean, headers: HttpHeaders): Observable<T> {
    return this.customRequest("post", url, params, headers, authenticationRequired, body);
  }
  public put(body: Entity, url: string = this.remoteUrl, params: any = {}, headers: HttpHeaders = this.defaultHttpHeaders): Observable<TEntity> {
    return this.putCustom(body, url, params, this.postAuthenticationRequired, headers);
  }
  public putCustom<T>(body: any, url: string, params: any, authenticationRequired: boolean, headers: HttpHeaders): Observable<T> {
    return this.customRequest<T>("put", url, params, headers, authenticationRequired, body);
  }
  public postOrPut(model: TEntity, url: string = this.remoteUrl, params: any = {}, headers: HttpHeaders = this.defaultHttpHeaders): Observable<TEntity> {
    if (model.id)
      return this.put(model, url, params, headers);
    else
      return this.post(model, url, params, headers);
  }
  public index(params: any = {}): Observable<EntityIndex<TEntity>> {
    if (params && params[ParameterNames.updateAuthorizationFilter])
      this.getAuthenticationRequired = true;
    return this.getCustom<EntityIndex<TEntity>>(this.remoteUrl, params, this.defaultHttpHeaders, this.getAuthenticationRequired);
  }
  public get(id: number, params: any = {}): Observable<TEntity> {
    return this.getCustom(this.getUrlWithId(id), params, this.defaultHttpHeaders, this.getAuthenticationRequired);
  }
  public getDetail<TDetail>(id: number, params: any = {}): Observable<TDetail> {
    return this.getCustom(this.getUrlWithIdWithSuffix(id, "detail"), params, this.defaultHttpHeaders, this.getAuthenticationRequired);
  }
  public delete(id: number): Observable<string> {
    return this.deleteCustom(this.getUrlWithId(id), {}, this.defaultHttpHeaders, this.deleteAuthenticationRequired);
  }
  public deleteCustom<T>(url: string, params: any, headers: HttpHeaders, authenticationRequired: boolean): Observable<T> {
    return this.customRequest("delete", url, params, headers, authenticationRequired, {});
  }
  public getCustom<T>(url: string, params: any, headers: HttpHeaders, authenticationRequired: boolean): Observable<T> {
    return this.customRequest("get", url, params, headers, authenticationRequired, {});
  }
  protected customRequest<TResult>(
    requestType: string,
    url: string,
    params: any,
    headers: any,
    authenticationRequired: boolean,
    body: any,
    retryCount: number = 1,
    customSource: Subject<TResult> = null): Observable<TResult> {
    console.log("customRequest enter");
    if (!customSource)
      customSource = new Subject<TResult>();

    if (authenticationRequired)
      headers = this.identityService.getHeadersWithAuthInfo(headers);
    let operation = this.getOperation(requestType, url, params, headers, body);
    this.messageService.sendTrace(`${this.serviceName} ${requestType} ${url} ${(body.toString())} request ${retryCount} time`);
    operation
      .retry(1)
      .subscribe((entity: TResult) => {
        this.messageService.sendTrace(`${this.serviceName} ${requestType} ${url} ${(entity as any).id} success from ${retryCount} time`);
        customSource.next(entity);
      },
      errorResponse => {
       // console.log("customRequest errorResponse enter");
        if (errorResponse.status == 401) {
          console.log("customRequest errorResponse  401 enter");
          if (retryCount < this.retryLimit) {
           // console.log("customRequest errorResponse  401 < 3 enter");
            this.identityService.ensureSignIn().subscribe((success) => {
              //console.log("customRequest errorResponse  401 < 3  subscribe enter"); 
                //  this.messageService.sendTrace(`log in success from ${retryCount} repeat`);
                  this.customRequest(requestType, url, params, headers, authenticationRequired, body, ++retryCount, customSource);
              });
          } else {
            //console.log("customRequest errorResponse  401 > 3 enter");
              this.handleError(customSource, errorResponse);
            }
        } else {
          //console.log("customRequest errorResponse  not 401  enter");
            //throw errorResponse;
            this.handleError(customSource, errorResponse);
          }
      });
    console.log("customRequest leave");
    return customSource.asObservable();
  }
  protected getOperation<TResult>(type: string, url: string, params: any, headers: HttpHeaders, body: any): Observable<TResult> {
    let operation: Observable<TResult>;
    switch (type) {
      case "get": {
        operation = this.http.get<TResult>(url, { headers: headers, params: params });
        break;
      }
      case "post": {
        operation = this.http.post<TResult>(url, body, { headers: headers, params: params });
        break;
      }
      case "put": {
        operation = this.http.put<TResult>(url, body, { headers: headers, params: params });
        break;
      }
      case "delete": {
        operation = this.http.delete<TResult>(url, { headers: headers, params: params });
        break;
      }
    }
    return operation;
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
  protected handleError<T>(customSource: Subject<T>, errorResponse: any) {
    let message = "";
    switch (errorResponse.status) {
      case 409:
        message = `Error validating data: ${errorResponse.error.message}`;
        break;
      default:
        customSource.next(null);
        throw errorResponse;
    }
    this.messageService.sendError(message);
    customSource.next(null);
  }
}
