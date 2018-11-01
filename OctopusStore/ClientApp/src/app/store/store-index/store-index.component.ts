import { Component, OnInit } from '@angular/core';
import { StoreService } from '../../services/store.service';
import { Store } from '../../view-models/store/store';
import { StoreIndex } from '../../view-models/store/store-index';
import { ParameterNames } from '../../services/parameter-names';

@Component({
  selector: 'app-store-index',
  templateUrl: './store-index.component.html',
  styleUrls: ['./store-index.component.css']
})
export class StoreIndexComponent implements OnInit {
  storeIndex: StoreIndex;
  //detailsLink: string = this.storeService.getUrlWithParameter(ParameterNames.detail);

  constructor(
    private storeService: StoreService
  ) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  getDetailUrl(store: Store): string {
    return `${store.id}/${ParameterNames.detail}`;
  }

  initializeComponent() {
    this.storeService.index().subscribe((storeIndex: StoreIndex) => {
      this.storeIndex = new StoreIndex(storeIndex);
    })
  }
}
