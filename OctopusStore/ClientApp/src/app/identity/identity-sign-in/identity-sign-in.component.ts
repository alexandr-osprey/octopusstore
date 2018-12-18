import { Component, OnInit } from '@angular/core';
import 'rxjs/add/operator/catch';
import { Router } from '@angular/router';
import { IdentityService } from '../identity.service';
import { MessageService } from 'src/app/message/message.service';
import { Credentials } from '../credentials';
import { TokenPair } from '../token-pair';


@Component({
  selector: 'app-identity-sign-in',
  templateUrl: './identity-sign-in.component.html',
  styleUrls: ['./identity-sign-in.component.css']
})
export class IdentitySignInComponent implements OnInit {
  credentials: Credentials;

  constructor(
    private identityService: IdentityService,
    private router: Router,
    private messageService: MessageService)
  {
  }

  ngOnInit() {
    this.credentials = new Credentials();
  }

  signIn() {
    this.identityService.signIn(this.credentials)
      .subscribe((data: TokenPair) => {
        if (data) {
          this.messageService.sendSuccess("Signed In successfully");
          this.router.navigate(['/']);
        }
      },
      (errorResponse) => {
        this.messageService.sendError(errorResponse.error.message);
      });
  }
}
