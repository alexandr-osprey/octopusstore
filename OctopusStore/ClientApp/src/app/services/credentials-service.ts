import { HttpHeaders, HttpClient } from "@angular/common/http";
import { MessageService } from "./message.service";
import { Observable } from "rxjs/internal/Observable";
import { of } from "rxjs";
import { Credentials } from "../view-models/credentials/credentials";
import { tap, catchError } from "rxjs/operators";
import { Injectable } from "@angular/core";
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class CredentialsService {
  protected remoteUrl: string = 'api/credentials';
  protected serviceName: string = "Credentials service";
  public static get TOKEN_NAME(): string { return 'token' };
  public static get EXPIRATION_NAME(): string { return 'expires_at' };

  protected httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
  }

  createOrUpdate(credentials: Credentials): Observable<string> {
    return this.http.post<string>(this.remoteUrl, credentials, this.httpOptions)
      .pipe(
      tap(c => { this.log(`${this.serviceName} created or updated user ${credentials.email}`) }),
        catchError(this.handleError<any>(`create or update error user ${credentials.email}`)));
  }
  login(credentials: Credentials): Observable<string> {
    return this.http.post<string>(this.remoteUrl + '/login', credentials)
      .pipe(
      tap(t => {
        this.setToken(t);
        this.setExpirationDate(t);
        this.log(`${this.serviceName} logged in user ${credentials.email}`);
      }),
      catchError(this.handleError<any>(`login error user ${credentials.email}`)));
  }
  delete(email: string): Observable<any> {
    return this.http.delete<any>(this.getUrlWithEmail(email), this.httpOptions)
      .pipe(
      tap(_ => this.log(`${this.serviceName} deleted user ${email}`)),
      catchError(this.handleError<any>(`delete error user ${email}`)));
  }
  setToken(token: string) {
    localStorage.setItem(CredentialsService.TOKEN_NAME, token);
  }
  getToken(): string {
    return localStorage.getItem(CredentialsService.TOKEN_NAME);
  }
  setExpirationDate(token: string) {
    let expirationDate = new Date();
    expirationDate.setSeconds(expirationDate.getTime() + this.getExpirationDate(token).getSeconds() * 1000);
    localStorage.setItem(CredentialsService.EXPIRATION_NAME, expirationDate.toUTCString());
  }
  getExpirationDate(token: string): Date {
    const decoded = jwt_decode(token);
    if (decoded.exp == null) return new Date(0);
    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }
  isTokenExpired(token?: string) {
    if (!token) token = this.getToken();
    if (!token) return true;
    const date = this.getExpirationDate(token);
    if (date == undefined) return true;
    return !(date > new Date());
  }

  getUrlWithEmail(email: string, url: string = this.remoteUrl) {
    return `${url}/${email}`;
  }

  protected handleError<T>(operation = 'operation', result?: T) {
    return (errorResponse: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(errorResponse); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed. Message: ${errorResponse.message}; Error: ${errorResponse.error}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
  protected log(message: string) {
    this.messageService.add(this.serviceName + ' ' + message);
    console.log(message);
  }
}
