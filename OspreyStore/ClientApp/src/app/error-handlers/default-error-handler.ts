import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from '../message/message.service';

@Injectable()
export class DefaultErrorHandler implements ErrorHandler {

  constructor(
    private injector: Injector,
    private messageService: MessageService,
    private zone: NgZone) { }

  handleError(errorResponse) {
    const router = this.injector.get(Router);
    let message = '';
    
    let urlToNavigateTo = "";
    let status = errorResponse.status || (errorResponse.rejection && errorResponse.rejection.status);
    switch (status) {
      case 401:
        message = "Please sign in";
        urlToNavigateTo = '/signIn/';
        break;
      case 403:
        // report a bug later on!
        //message = "You are not authorized for this operation";
        //urlToNavigateTo = '/';
        break;
      case 404:
        message = "Requested resource not found";
        urlToNavigateTo = '/pageNotFound';
        break;
      default:
        if (errorResponse.message) {
          message = errorResponse.message;
          if (errorResponse.stack)
            message += errorResponse.stack;
        }
        if (errorResponse.error) {
          if (errorResponse.error.message)
            message = errorResponse.error.message;
        } 
        urlToNavigateTo = '/';
        break;
    }
    if (message.length)
      this.messageService.sendError(message);
    if (urlToNavigateTo.length) {
      this.zone.run(() => { this.injector.get(Router).navigate([urlToNavigateTo]); });
    }
      
    return errorResponse;
  }
}
