import { Component, OnInit, Input } from '@angular/core';
import { StoreService } from '../../services/store.service';
import { ActivatedRoute } from '@angular/router';
import { Index } from '../../view-models';
import { Observable } from 'rxjs';
import { Store } from '../../view-models/store';

@Component({
  selector: 'app-store-administrators',
  templateUrl: './store-administrators.component.html',
  styleUrls: ['./store-administrators.component.css']
})
export class StoreAdministratorsComponent implements OnInit {
  @Input() store: Store;
  storeAdministrators: StoreAdministrator[] = [];
  addedStoreAdministrators: StoreAdministrator[] = [];

  constructor(private storeService: StoreService,
    private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let storeId = +this.route.snapshot.paramMap.get('id');
    this.storeAdministrators = [];
    this.addedStoreAdministrators = [];
    if (storeId) {
      this.storeService.indexStoreAdministrators(storeId).subscribe((data: Index<string>) => {
        if (data) {
          data.entities.forEach(e => this.storeAdministrators.push(new StoreAdministrator(e)));
        }
      });
    }
  }

  saveAddedAdministrators() {
    let storeId = +this.route.snapshot.paramMap.get('id');
    let observables: Observable<string>[] = [];
    this.addedStoreAdministrators.forEach(a => observables.push(this.storeService.postStoreAdministrator(storeId, a.email)));
    Observable.zip(...observables).subscribe(administrators => {
      //let added: StoreAdministrator[] = [];
      //administrators.forEach(a => added.push(new StoreAdministrator(a)));
      //this.storeAdministrators = this.storeAdministrators.concat(added);
      this.initializeComponent();
    });
  }
  removeStoreAdministrator(administrator: StoreAdministrator) {
    let storeId = +this.route.snapshot.paramMap.get('id');
    //let index = this.storeAdministrators.indexOf(administrator);
    this.storeService.deleteStoreAdministrator(storeId, administrator.email).subscribe(answer => {
      this.storeAdministrators = this.storeAdministrators.filter(a => a.email != administrator.email);
    });
  }
  addStoreAdministrator() {
    this.addedStoreAdministrators.push(new StoreAdministrator(""));
  }
}

class StoreAdministrator {
  email: string;
  constructor(email: string) {
    this.email = email;
  }
}
