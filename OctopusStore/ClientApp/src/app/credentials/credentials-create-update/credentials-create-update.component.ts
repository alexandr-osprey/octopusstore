import { Component, OnInit } from '@angular/core';
import { Credentials } from '../../view-models/credentials/credentials';
import { CredentialsService } from '../../services/credentials-service';
import { pipe } from 'rxjs';
import { tap } from 'rxjs/operators';
import Popper from 'popper.js';
import 'rxjs/add/operator/catch';


@Component({
  selector: 'app-credentials-create-update',
  templateUrl: './credentials-create-update.component.html',
  styleUrls: ['./credentials-create-update.component.css']
})
export class CredentialsCreateUpdateComponent implements OnInit {

  credentials: Credentials;
  errorMessage: string;

  constructor(private credentialsService: CredentialsService) { }

  ngOnInit() {
    this.credentials = new Credentials(); 
  }

  save() {
    this.credentialsService.createOrUpdate(this.credentials).subscribe(
      (token: string) => {
        var errorMessagePopper = new Popper(credentialsForm, onLeftPopper);
        this.errorMessage = this.credentialsService.lastError;
      },
      (errorResponse) => {

      }
    })
  }
}
