import { Component, OnInit } from '@angular/core';
import { Store } from '../../view-models/store/store';
import { StoreService } from '../../services/store.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-store-create-update',
  templateUrl: './store-create-update.component.html',
  styleUrls: ['./store-create-update.component.css']
})
export class StoreCreateUpdateComponent implements OnInit {

  store: Store;
  constructor(
    private storeService: StoreService,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let id = +this.route.snapshot.paramMap.get('id');
    if (id) {
      this.storeService.get(id).subscribe((data: Store) => {
        this.store = data;
      });
    } else {
      this.store = new Store();
    }
  }

  createOrUpdate() {
    this.storeService.createOrUpdate(this.store).subscribe((data: Store) => {
      this.store = data;
      this.router.navigate([`/stores/${data.id}/details`]);
    });
  }

}
