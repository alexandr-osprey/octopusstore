import * as jwt_decode from 'jwt-decode';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MessageService } from '../services/message.service';

export class TokenPair {
  public token: string;
  public refreshToken: string;

  public constructor(init?: Partial<TokenPair>) {
    Object.assign(this, init);
  }
}

export class TokenManager {
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
  }
  public isTokenInvalid(token?: string): boolean {
    if (!this.tokenExists())
      return true;
    const date = this.getExpirationDate(token);
    if (date == undefined) return true;
    //console.log("t: " + date);
    //console.log("now: " + new Date());
    //console.log(!(date > new Date()));
    return !(date > new Date());
  }
  public clear(): void {
    localStorage.removeItem(this.TOKEN_NAME);
    localStorage.removeItem(this.REFRESH_TOKEN_NAME);
  }
  //public setTokenPairExpirationDate() {
  //  console.log("token exp date: " + this.getExpirationDate(this.token).toUTCString());
  //  localStorage.setItem(this.EXPIRATION_NAME, this.getExpirationDate(this.token).toUTCString());
  //  console.log("refreshToken exp date: " + this.getExpirationDate(this.refreshToken).toUTCString());
  //  localStorage.setItem(this.REFRESH_EXPIRATION_NAME, this.getExpirationDate(this.refreshToken).toUTCString());
  //}
  protected getExpirationDate(token: string): Date {
    let decoded = jwt_decode(token);
    if (decoded.exp == null)
      return new Date(0);
    let date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }
  public getHeadersWithToken(headers?: HttpHeaders): HttpHeaders {
    if (!headers)
      headers = new HttpHeaders();
    return headers.set(this.AUTH_HEADER_KEY, `${this.AUTH_PREFIX} ${this.getToken()}`);
  }
  //public setTokenPair(tokenPair: TokenPair) {
  //  localStorage.setItem(this.TOKEN_NAME, tokenPair.token);
  //  localStorage.setItem(this.REFRESH_TOKEN_NAME, tokenPair.refreshToken);
  //  //this.setSignInState(true);
  //}


  public refresh(): Observable<TokenPair> {
    //console.log("refresh enter");
    //console.log("refreshToken date: " + this.getExpirationDate(this.getRefreshToken()));
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
    //    console.log("refresh delay enter");
        sub.error(new HttpErrorResponse({ status: 401, statusText: "no saved refreshToken" }))
      });
  //    console.log("refresh leave");
      return sub.asObservable();
    }
  }
  protected delay(ms: number): Promise<{}> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
