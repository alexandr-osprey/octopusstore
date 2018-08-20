import { Component, OnInit } from '@angular/core';
import { Credentials } from '../../view-models/credentials/credentials';

@Component({
  selector: 'app-credentials-create-update',
  templateUrl: './credentials-create-update.component.html',
  styleUrls: ['./credentials-create-update.component.css']
})
export class CredentialsCreateUpdateComponent implements OnInit {

  credentials: Credentials;

  constructor() { }

  ngOnInit() {
    this.credentials = new Credentials(); 
  }

  save() {

  }
}
