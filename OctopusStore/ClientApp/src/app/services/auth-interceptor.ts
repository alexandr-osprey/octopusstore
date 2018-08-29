import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpEvent, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CredentialsService } from './credentials-service';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<any>,
    next: HttpHandler): Observable<HttpEvent<any>>{
    const token = localStorage.getItem(CredentialsService.TOKEN_NAME);
    if (token) {
      const cloned = request.clone({
        headers: request.headers.set("Authorization",
          "Bearer " + token)
      });
      return next.handle(cloned);
    } else {
      return next.handle(request);
    }
  }
}
