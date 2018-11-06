import { Component, OnInit } from '@angular/core';
import { Credentials } from '../../view-models/identity/credentials';
import { IdentityService } from '../../services/identity.service';
import 'rxjs/add/operator/catch';
import { MessageService } from '../../services/message.service';
import { Router } from '@angular/router';
import { TokenPair } from '../../view-models/identity/token-pair';


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
