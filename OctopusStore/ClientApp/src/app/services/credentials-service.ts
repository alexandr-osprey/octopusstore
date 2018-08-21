import { HttpHeaders, HttpClient } from "@angular/common/http";
import { MessageService } from "./message.service";
import { Observable } from "rxjs/internal/Observable";
import { of } from "rxjs";
import { Credentials } from "../view-models/credentials/credentials";
import { tap, catchError } from "rxjs/operators";

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

  public createOrUpdate(credentials: Credentials): Observable<Credentials> {
    if (credentials.guid)
      return this.create(credentials);
    else
      return this.update(credentials);
  }
  public create(credentials: Credentials): Observable<Credentials> {
    let created = this.http.post<Credentials>(this.remoteUrl, credentials, this.httpOptions)
      .pipe(
      tap(c => this.log(`${this.serviceName} created guid= ${c.email}`)),
        catchError(this.handleError<any>(`create error ` + credentials)));
    return created;
  }

  public update(credentials: Credentials): Observable<Credentials> {
    let updated = this.http.put<Credentials>(this.getUrlWithGuid(credentials.guid, this.remoteUrl), credentials, this.httpOptions)
      .pipe(
      tap(c => { this.log(`${this.serviceName} updated guid=${c.guid}`) }),
      catchError(this.handleError<any>(`update error guid=${credentials.guid}`)));
    return updated;
  }

  public delete(guid: string): Observable<any> {
    let result = this.http.delete<any>(this.getUrlWithGuid(guid), this.httpOptions)
      .pipe(
      tap(_ => this.log(`${this.serviceName} deleted guid=${guid}`)),
      catchError(this.handleError<any>(`delete error guid=${guid}`)));
    return result;
  }

  public getUrlWithGuid(guid: string, url: string = this.remoteUrl) {
    return `${url}/${guid}`;
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
