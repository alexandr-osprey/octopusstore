import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../services/identity.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  signedInSubscription: Subscription;
  signedIn: boolean;
  email: string;
  constructor(
    private identityService: IdentityService,
    private router: Router)
  {
    this.signedIn = this.identityService.signedIn;
    this.email = this.identityService.currentUserEmail;
    this.signedInSubscription = this.identityService.signedIn$.subscribe(
      signedIn => {
        this.signedIn = signedIn;
        this.email = this.identityService.currentUserEmail;
      });
  }

  ngOnInit() {
  }

  signOut() {
    this.identityService.signOut();
    this.router.navigate([""]);
  }
}
