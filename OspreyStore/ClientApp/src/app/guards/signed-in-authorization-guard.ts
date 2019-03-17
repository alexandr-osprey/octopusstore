import { Injectable } from '@angular/core';
import { CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router'
import { IdentityService } from '../identity/identity.service';


@Injectable()
export class SignedInAuthorizationGuard implements CanActivate {
  constructor(
    private identityService: IdentityService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.identityService.ensureSignIn().subscribe((success) => {
      console.log(success);
    }, error => {
      return false;
    });
    return true;
  }
}
