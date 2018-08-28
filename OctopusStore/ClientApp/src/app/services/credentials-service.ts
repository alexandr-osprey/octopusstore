import { HttpHeaders, HttpClient } from "@angular/common/http";
import { MessageService } from "./message.service";
import { Observable } from "rxjs/internal/Observable";
import { of } from "rxjs";
import { Credentials } from "../view-models/credentials/credentials";
import { tap, catchError } from "rxjs/operators";
import { UserToken } from "../view-models/credentials/user-token";
import { Injectable } from "@angular/core";
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class CredentialsService {
  protected remoteUrl: string = 'api/credentials';
  protected serviceName: string = "Credentials service";
  protected httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
  }

  public createOrUpdate(credentials: Credentials): Observable<UserToken> {
    let token = this.http.post<UserToken>(this.remoteUrl, credentials, this.httpOptions)
      .pipe(
      tap(c => { this.log(`${this.serviceName} created or updated user ${credentials.email}`) }),
        catchError(this.handleError<any>(`create or update error user ${credentials.email}`)));
    return token;
  }

  public delete(email: string): Observable<any> {
    let result = this.http.delete<any>(this.getUrlWithEmail(email), this.httpOptions)
      .pipe(
      tap(_ => this.log(`${this.serviceName} deleted user ${email}`)),
      catchError(this.handleError<any>(`delete error user ${email}`)));
    return result;
  }

  private setSession(token: string) {
    localStorage.setItem('token', token);

  }

  private getTokenExpirationDate(token: string): Date {
    const decoded = jwt_decode(token);
    if (decoded.exp == null) return new Date(0);
    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  public getUrlWithEmail(email: string, url: string = this.remoteUrl) {
    return `${url}/${email}`;
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
