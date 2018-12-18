import { Component, OnInit } from '@angular/core';
import 'rxjs/add/operator/catch';
import { Router } from '@angular/router';
import 'rxjs/add/operator/retryWhen';
import { Credentials } from '../credentials';
import { IdentityService } from '../identity.service';
import { MessageService } from 'src/app/message/message.service';
import { TokenPair } from '../token-pair';


@Component({
  selector: 'app-identity-sign-up',
  templateUrl: './identity-sign-up.component.html',
  styleUrls: ['./identity-sign-up.component.css']
})
export class IdentitySignUpComponent implements OnInit {
  credentials: Credentials;
  constructor(
    private identityService: IdentityService,
    private messageService: MessageService,
    private router: Router)
  {
  }

  ngOnInit() {
    this.credentials = new Credentials();
  }

  create() {
    this.identityService.signUp(this.credentials)
      .subscribe((data: TokenPair) => {
          this.messageService.sendSuccess("User created successfully");
          this.router.navigate(["/items"]);
      },
      (errorResponse) => {
        this.messageService.sendError(errorResponse.error.message)
      });
  }

  getAuthData() {
    this.identityService.authGet().subscribe(data => {
    }, errorResponse => this.messageService.sendError(errorResponse.error.message));
  }
  ensureSignIn() {
    this.identityService.ensureSignIn();
  }
  signIn() {
    this.identityService.signIn(new Credentials({ email: "john@mail.com", password: "Password1!" })).subscribe(data => {
      this.messageService.sendSuccess("Sucessfully signed in.")
    }, errorResponse => this.messageService.sendError(errorResponse.error.message));
  }
  signOut() {
    this.identityService.signOut();
  }
}
