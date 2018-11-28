import { Injectable } from '@angular/core';
import { CanActivate, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router'
import { IdentityService } from '../services/identity.service';


@Injectable()
export class CreateUpdateAuthorizationGuard implements CanActivate {
  constructor(
    private identityService: IdentityService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.identityService.checkCreateUpdateAuthorization(state.url, true).subscribe(result => {
      if (!result) {
        return false;
      }
    });
    return true;
  }
}
