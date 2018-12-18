import * as jwt_decode from 'jwt-decode';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MessageService } from '../message/message.service';
import { TokenPair } from './token-pair';

export class TokenService {
  public TOKEN_NAME: string = 'token';
  public REFRESH_TOKEN_NAME: string = 'refreshToken';
  //public EXPIRATION_NAME: string = 'expiresAt';
  //public REFRESH_EXPIRATION_NAME: string = 'refreshExpiresAt';
  public AUTH_HEADER_KEY: string = 'Authorization';
  public AUTH_PREFIX: string = 'Bearer';

  public tokenPair: TokenPair;

  public constructor(
    protected http: HttpClient,
    protected messageService: MessageService,
    protected remoteUrl: string,
    protected defaultHttpHeaders: HttpHeaders) {
  }

  public setTokenPair(init?: Partial<TokenPair>) {
    this.tokenPair = new TokenPair(init);
    localStorage.setItem(this.TOKEN_NAME, this.tokenPair.token);
    localStorage.setItem(this.REFRESH_TOKEN_NAME, this.tokenPair.refreshToken);
  }

  public getToken(): string {
    return localStorage.getItem(this.TOKEN_NAME);
  }
  public getRefreshToken(): string {
    return localStorage.getItem(this.REFRESH_TOKEN_NAME);
  }
  public getHeadersWithTokenPair(headers: HttpHeaders): HttpHeaders {
    return this.getHeadersWithToken(headers)
      .append(this.REFRESH_TOKEN_NAME, this.getRefreshToken());
  }
  public tokenExists(token?: string): boolean {
    if (!token)
      token = this.getToken();
    if (!token)
      return false;
    return true;
  }
  public isTokenInvalid(): boolean {
    if (!this.tokenExists())
      return true;
    if (this.tokenPair && this.tokenPair.invalid)
      return true;
    let token = this.getToken();
    const date = this.getExpirationDate(token);
    if (date == undefined) return true;
    //console.log("t: " + date);
    //console.log("now: " + new Date());
    //console.log(!(date > new Date()));
    return !(date > new Date());
  }
  public clear(): void {
    this.clearToken();
    this.clearRefreshToken();
  }

  public getValue(key: string): any {
    let token = this.getToken();
    let result: any = null;
    if (token) {
      let decoded = jwt_decode(token);
      result = decoded[key]
    }
    return result;
  }

  protected getExpirationDate(token: string): Date {
    let decoded = jwt_decode(token);
    if (decoded.exp == null)
      return new Date(0);
    let date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  public clearToken() {
    localStorage.removeItem(this.TOKEN_NAME);
  }

  public clearRefreshToken() {
    localStorage.removeItem(this.REFRESH_TOKEN_NAME);
  }

  public markAsInvalid() {
    this.tokenPair.invalid = true;
  }


  public getHeadersWithToken(headers?: HttpHeaders): HttpHeaders {
    if (!headers)
      headers = new HttpHeaders();
    return headers.set(this.AUTH_HEADER_KEY, `${this.AUTH_PREFIX} ${this.getToken()}`);
  }


  public refresh(): Observable<TokenPair> {
    let headers = { headers: this.getHeadersWithTokenPair(this.defaultHttpHeaders) };
    if (this.getRefreshToken()) {
      return this.http.post<TokenPair>(this.remoteUrl + '/refreshToken', null, headers).pipe(
        tap(t => {
          this.setTokenPair(t);
          this.messageService.sendTrace("refresh token success");
        }));
    } else {
      let sub = new Subject<TokenPair>();
      this.delay(5).then(() => {
        sub.error(new HttpErrorResponse({ status: 401, statusText: "no saved refreshToken" }))
      });
      return sub.asObservable();
    }
  }
  protected delay(ms: number): Promise<{}> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
