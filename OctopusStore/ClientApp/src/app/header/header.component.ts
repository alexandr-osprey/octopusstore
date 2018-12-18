import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { IdentityService } from '../identity/identity.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  signedInSubscription: Subscription;
  signedIn: boolean;
  email: string;
  isContentAdministrator: boolean;

  constructor(
    private identityService: IdentityService,
    private router: Router)
  {
    this.signedIn = this.identityService.signedIn;
    this.email = this.identityService.currentUserEmail;
    this.isContentAdministrator = this.identityService.isContentAdministrator();
    this.signedInSubscription = this.identityService.signedIn$.subscribe(
      signedIn => {
        this.signedIn = signedIn;
        this.email = this.identityService.currentUserEmail;
        this.isContentAdministrator = this.identityService.isContentAdministrator();
      });
  }

  ngOnInit() {
  }

  signOut() {
    this.isContentAdministrator = false;
    this.identityService.signOut();
    this.router.navigate([""]);
  }
}
