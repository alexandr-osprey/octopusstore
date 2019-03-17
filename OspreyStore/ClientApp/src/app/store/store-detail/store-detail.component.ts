import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ParameterService, } from '../../parameter/parameter.service';
import { ParameterNames } from '../../parameter/parameter-names';
import { Store } from '../store';
import { StoreService } from '../store.service';
import { IdentityService } from 'src/app/identity/identity.service';

@Component({
  selector: 'app-store-detail',
  templateUrl: './store-detail.component.html',
  styleUrls: ['./store-detail.component.css']
})
export class StoreDetailComponent implements OnInit {
  storeDetail: Store;
  authorizedToUpdate: boolean = false;
  constructor(
    private storeService: StoreService,
    private parameterService: ParameterService,
    private route: ActivatedRoute,
    private identityService: IdentityService,
    private location: Location
  ) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let storeId = +this.route.snapshot.paramMap.get('id');
    this.parameterService.navigateWithUpdatedParams({ "storeId": storeId });
    if (storeId) {
      this.storeService.getDetail(storeId).subscribe((storeDetail: Store) => {
        this.storeDetail = new Store(storeDetail);
      });
      this.authorizedToUpdate = this.identityService.checkUpdateAuthorization(storeId);
    }
  }

  getUpdateLink(id: number) {
    if (id) {
      return this.storeService.getUrlWithIdWithSuffix(id, 'update', '/stores');
    }
    return "";
  }

  goBack(): void {
    this.location.back();
  }
}
