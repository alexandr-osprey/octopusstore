import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../services/identity-service';

@Component({
  selector: 'app-identity-sign-out',
  templateUrl: './identity-sign-out.component.html',
  styleUrls: ['./identity-sign-out.component.css']
})
export class IdentitySignOutComponent implements OnInit {

  constructor(private identityService: IdentityService) { }

  ngOnInit() {
  }
  signOut() {
    this.identityService.signOut();
  }
}
