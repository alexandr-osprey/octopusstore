import { Component, OnInit } from '@angular/core';
import { StoreDetail } from '../../view-models/store/store-detail';
import { StoreService } from '../../services/store.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ParameterService } from '../../services/parameter-service';
import { ParameterNames } from '../../services/parameter-names';

@Component({
  selector: 'app-store-details',
  templateUrl: './store-details.component.html',
  styleUrls: ['./store-details.component.css']
})
export class StoreDetailsComponent implements OnInit {
  public storeDetails: StoreDetail;

  constructor(
    private storeService: StoreService,
    private parameterService: ParameterService,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let storeId = +this.route.snapshot.paramMap.get('id');
    if (storeId) {
      this.storeService.getDetail(storeId).subscribe((data: StoreDetail) => {
        this.storeDetails = data;
      });
      this.parameterService.setParam(ParameterNames.storeId, storeId);
    }
  }

  goBack(): void {
    this.location.back();
  }
}
