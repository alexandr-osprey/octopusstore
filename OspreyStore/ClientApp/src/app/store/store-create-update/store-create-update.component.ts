import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { StoreService } from '../store.service';
import { MessageService } from 'src/app/message/message.service';
import { IdentityService } from 'src/app/identity/identity.service';
import { Store } from '../store';

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
    let isCreate = this.store.id == 0;
    this.storeService.postOrPut(this.store).subscribe((data: Store) => {
      if (data) {
        this.store = data;
        this.messageService.sendSuccess(`Store saved`);
        // token changes after store creation, need to update
        if (isCreate) {
          this.identityService.reSignIn().subscribe(() => {
            this.router.navigate([`/stores/${data.id}/detail`]);
          });
        } else {
          this.router.navigate([`/stores/${data.id}/detail`]);
        }
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
