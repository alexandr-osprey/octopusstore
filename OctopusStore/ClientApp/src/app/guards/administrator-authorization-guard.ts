import { Injectable } from '@angular/core';
import { CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot, Router } from '@angular/router'
import { IdentityService } from '../services/identity.service';
import { MessageService } from '../services/message.service';


@Injectable()
export class AdministratorAuthorizationGuard implements CanActivate {
  constructor(
    private identityService: IdentityService,
    private messageService: MessageService,
    private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (!this.identityService.isContentAdministrator()) {
      let message = "You are not authorized for this operation";
      this.messageService.sendError(message);
      this.router.navigate(['/']);
      throw message;
      return false;
    }
    return true;
  }
}
