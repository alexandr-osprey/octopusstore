import { Component, OnInit } from '@angular/core';
import { StoreDetails } from '../../view-models/store/store-details';
import { StoreService } from '../../services/store.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ParameterService } from '../../services/parameter-service';
import { ParameterNames } from '../../services/parameter-names';
import { IdentityService } from '../../services/identity-service';

@Component({
  selector: 'app-store-details',
  templateUrl: './store-details.component.html',
  styleUrls: ['./store-details.component.css']
})
export class StoreDetailsComponent implements OnInit {
  storeDetails: StoreDetails;
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
    this.parameterService.navigateWithUpdatedParam([ParameterNames.storeId, storeId]);
    if (storeId) {
      this.storeService.getDetail(storeId).subscribe((storeDetails: StoreDetails) => {
        this.storeDetails = new StoreDetails(storeDetails);
      });
      this.identityService.checkCreateUpdateAuthorization(this.getUpdateLink(storeId)).subscribe(result => {
        if (result)
          this.authorizedToUpdate = true;
      });
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
