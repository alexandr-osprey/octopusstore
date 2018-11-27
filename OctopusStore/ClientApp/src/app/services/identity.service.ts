import { HttpHeaders, HttpClient, HttpErrorResponse } from "@angular/common/http";
import { MessageService } from "./message.service";
import { Observable } from "rxjs/internal/Observable";
import { Credentials } from "../view-models/identity/credentials";
import { tap } from "rxjs/operators";
import { Injectable } from "@angular/core";
import 'rxjs/add/operator/retry';
import { Router } from "@angular/router";
import { Subject } from "rxjs";
import { TokenService } from "./token.service";
import { TokenPair } from "../view-models/identity/token-pair";

@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  protected retryLimit: number = 3;
  protected delayMilliseconds = 2 * 1000;
  protected remoteUrl: string = '/api/identity';
  protected serviceName: string = "Credentials service";
  protected AUTH_HEADER_KEY: string = 'Authorization';
  protected AUTH_PREFIX: string = 'Bearer';
  protected EMAIL_NAME: string = 'email';
  protected PASSWORD_NAME: string = 'password';
  protected tokenManager: TokenService;
  protected get defaultHttpHeaders(): HttpHeaders {
    return new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  public get currentUserEmail(): string {
    let email = localStorage.getItem(this.EMAIL_NAME);
    return email;
  }
  protected signedInSource = new Subject<boolean>();
  public signedIn$ = this.signedInSource.asObservable();
  protected _signedIn: boolean;
  public get signedIn(): boolean {
    return this._signedIn;
  }

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected messageService: MessageService) {
    this.tokenManager = new TokenService(this.http, this.messageService, this.remoteUrl, this.defaultHttpHeaders);
    this._signedIn = this.hasSavedCredentials();
    if (this.hasSavedCredentials())
      this.ensureSignIn().subscribe();
  }
  public signUp(credentials: Credentials): Observable<TokenPair> {
    return this.post(credentials, this.remoteUrl);
  }
  public signIn(credentials: Credentials): Observable<TokenPair> {
    return this.post(credentials, this.remoteUrl + '/signIn');
  }


  public signOut() {
    this.tokenManager.clear();
    this.setSignInState(false);
    if (this.hasSavedCredentials()) {
      localStorage.removeItem(this.EMAIL_NAME);
      localStorage.removeItem(this.PASSWORD_NAME);
    }
  }
  public ensureSignIn(): Observable<TokenPair> {
   // console.log("ensureSignIn enter");
    let subjectForResultNotifying = new Subject<TokenPair>();
    if (this.tokenManager.isTokenInvalid()) {
      this.tokenManager.refresh()
        .retry(this.retryLimit)
        .subscribe(
          (data: TokenPair) => {
            //console.log("ensureSignIn subscribe enter");
            this.signInSucceded(data);
            subjectForResultNotifying.next(data);
          },
          (refreshTokenErrorResponse: HttpErrorResponse) => {
         //   console.log("ensureSignIn refreshTokenErrorResponse enter");
            if (refreshTokenErrorResponse.status == 401) {
              this.reSignIn()
                //.pipe(
                //catchError(error => {
                //    console.log("gotcha!");
                //    return null;
                //  }))
                .subscribe(data => {
                  console.log("ensureSignIn refreshTokenErrorResponse subscribe enter");
                  subjectForResultNotifying.next(new TokenPair(data));
                }, resignInErrorResponse => {

                });
            } else {
            //  console.log("ensureSignIn refreshTokenErrorResponse not 401 enter");
              this.signInFailed(refreshTokenErrorResponse);
            }
          });
    } else {
      this.delay(5).then(() => {
       // console.log("ensureSignIn delay enter");
        subjectForResultNotifying.next(this.tokenManager.tokenPair);
      });
    }
    return subjectForResultNotifying.asObservable();
  }


  public refreshToken(): Observable<TokenPair> {
    this.tokenManager.markAsInvalid();
    return this.ensureSignIn();
  }
  public authGet(): Observable<string> {
    this.tokenManager.isTokenInvalid;
    let headers = this.tokenManager.getHeadersWithToken(this.defaultHttpHeaders);
    return this.http.get<string>('/api/credentials', { headers: headers });
  }

  public getUserAdministredStoreIds(): number[] {
    let result: number[] = [];
    let tokenValue = this.tokenManager.getValue("StoreAdministrator");
    if (tokenValue) {
      if (Array.isArray(tokenValue)) {
        for (let i = 0; i < tokenValue.length; i++) {
          result.push(+tokenValue[i]);
        }
      }
      else {
        result.push(+tokenValue);
      }
    }
    return result;
  }

  public checkCreateUpdateAuthorization(url: string, throwError: boolean = false): Observable<boolean> {
    let resultSubject = new Subject<boolean>();
    if (!(url && this.hasSavedCredentials())) {
      this.delay(5).then(() => resultSubject.next(false));
    } else {
      this.ensureSignIn()
        .subscribe(ensureResult => {
          let parts = url.split("/", 4);
          if (parts.length >= 4 && parts[3] == 'update') {
            let urlToCheck = `/api/${parts[1]}/${parts[2]}/checkUpdateAuthorization`;
            this.http.get(urlToCheck, { headers: this.tokenManager.getHeadersWithToken() })
              .retry(1)
              .subscribe(
                response => {
                  if (response)
                    resultSubject.next(true)
                }, errorResponse => {
                  resultSubject.next(false);
                  if (throwError) {
                    this.messageService.sendError("You are not authorized for this operation");
                    this.router.navigate(['/']);
                    throw errorResponse;
                  }
                });
          } else if (parts.length >= 3 && parts[2] == 'create') {
            resultSubject.next(true);
          } else {
            resultSubject.next(false);
          }
        });
    }
    return resultSubject.asObservable();
  }

  public getHeadersWithAuthInfo(headers?: HttpHeaders): HttpHeaders {
    return this.tokenManager.getHeadersWithToken(headers);
  }

  protected post(credentials: Credentials, url: string): Observable<TokenPair> {
    return this.http.post<TokenPair>(url, credentials, { headers: this.defaultHttpHeaders })
      .retry(this.retryLimit)
      .pipe(
        tap(tokenPair => this.signInSucceded(tokenPair, credentials),
          signInErrorResponse => this.signInFailed(signInErrorResponse)));
  }

  public hasSavedCredentials(): boolean {
    let email = localStorage.getItem(this.EMAIL_NAME);
    let password = localStorage.getItem(this.PASSWORD_NAME);
    return (email != null && password != null);
  }

  public checkUpdateAuthorization(storeId: number): boolean {
    let stores = this.getUserAdministredStoreIds();
    let allowed = stores.findIndex(s => s == storeId) >= 0;
    allowed = allowed || this.isContentAdministrator();
    return allowed;
  }

  public isContentAdministrator(): boolean {
    let value = this.tokenManager.getValue("Administrator");
    let contentAdministrator = value == "Content";
    return contentAdministrator;
  }

  protected signInFailed(errorResponse: HttpErrorResponse): void {
  //  console.log("signInFailed enter");
    //this.router.navigate(['signIn']);
    //subject.error(errorResponse);
    this.signOut();
    //this.messageService.sendError("Invalid credentials");
    //this.setSignInState(false);
    throw errorResponse;
  }
  protected setSignInState(state: boolean): void {
    if (state != this._signedIn) {
      this._signedIn = state;
      this.signedInSource.next(state);
    }
  }
  protected signInSucceded(tokenPair: TokenPair, credentials?: Credentials): void {
    this.tokenManager.setTokenPair(tokenPair);
    //this.setTokenPair(tokenPair);
    //this.setTokenPairExpirationDate(tokenPair);
    if (credentials) {
      localStorage.setItem(this.EMAIL_NAME, credentials.email);
      localStorage.setItem(this.PASSWORD_NAME, credentials.password);
    }
    this.setSignInState(true);
    //this.log(`${this.serviceName} logged in user ${credentials.email}`);
  }
  protected reSignIn(): Observable<TokenPair> {
    //console.log("reSignIn enter");
    if (this.hasSavedCredentials()) {
      let email = localStorage.getItem(this.EMAIL_NAME);
      let password = localStorage.getItem(this.PASSWORD_NAME);
      return this.signIn(new Credentials({ email: email, password: password }));
    } else {
      let result = new Subject<TokenPair>();
      let errorResponse = new HttpErrorResponse({ status: 401, statusText: "no saved credentials" });
      this.delay(5).then(() => {
        //console.log("reSignIn delay enter");
        this.signInFailed(errorResponse);
        result.error(errorResponse);
      });
      return result.asObservable();
    }
  }
  protected delay(ms: number): Promise<{}> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
