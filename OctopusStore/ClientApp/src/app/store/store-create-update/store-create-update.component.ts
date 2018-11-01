import { Component, OnInit } from '@angular/core';
import { Store } from '../../view-models/store/store';
import { StoreService } from '../../services/store.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { MessageService } from '../../services/message.service';
import { IdentityService } from '../../services/identity-service';

@Component({
  selector: 'app-store-create-update',
  templateUrl: './store-create-update.component.html',
  styleUrls: ['./store-create-update.component.css']
})
export class StoreCreateUpdateComponent implements OnInit {
  storeSubscription: Subscription;
  store: Store;

  constructor(
    private storeService: StoreService,
    private router: Router,
    private messageService: MessageService,
    private identityService: IdentityService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let id = +this.route.snapshot.paramMap.get('id');
    if (id) {
      this.storeService.get(id).subscribe((data: Store) => {
        if (data) {
          this.store = new Store(data);
        }
      });
    } else {
      this.store = new Store();
    }
  }

  createOrUpdate() {
    this.storeService.postOrPut(this.store).subscribe((data: Store) => {
      if (data) {
        this.store = data;
        this.messageService.sendSuccess(`Store saved`);
        this.router.navigate([`/stores/${data.id}/detail`]);
      }
    });
  }
  delete() {
    if (this.store.id) {
      this.storeService.delete(this.store.id).subscribe((data) => {
        if (data) {
          this.messageService.sendSuccess(`Store deleted`);
        }
      });
    }
  }
}
