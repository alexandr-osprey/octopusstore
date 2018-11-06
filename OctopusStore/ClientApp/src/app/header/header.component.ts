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
  signedIn: boolean = true;
  email: string;
  constructor(
    private identityService: IdentityService,
    private router: Router)
  {
    
    this.signedInSubscription = this.identityService.signedIn$.subscribe(
      signedIn => {
        this.signedIn = signedIn;
        this.email = this.identityService.currentUserEmail;
      });
    this.identityService.ensureSignIn().subscribe(_ => {

    });
  }

  ngOnInit() {
  }
  signOut() {
    this.identityService.signOut();
    this.router.navigate([""]);
  }
}
