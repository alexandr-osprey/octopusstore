import { Component, OnInit } from '@angular/core';
import { Credentials } from '../../view-models/credentials/credentials';
import { CredentialsService } from '../../services/credentials-service';
import { UserToken } from '../../view-models/credentials/user-token';

@Component({
  selector: 'app-credentials-create-update',
  templateUrl: './credentials-create-update.component.html',
  styleUrls: ['./credentials-create-update.component.css']
})
export class CredentialsCreateUpdateComponent implements OnInit {

  credentials: Credentials;

  constructor(private credentialsService: CredentialsService) { }

  ngOnInit() {
    this.credentials = new Credentials(); 
  }

  save() {
    this.credentialsService.createOrUpdate(this.credentials).subscribe((token: string) => {
      //console.log(data.token);
    });
  }
}
