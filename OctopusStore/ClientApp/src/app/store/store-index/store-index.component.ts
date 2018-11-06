import { Component, OnInit } from '@angular/core';
import { StoreService } from '../../services/store.service';
import { Store } from '../../view-models/store/store';
import { ParameterNames } from '../../services/parameter-names';
import { EntityIndex } from '../../view-models/entity/entity-index';

@Component({
  selector: 'app-store-index',
  templateUrl: './store-index.component.html',
  styleUrls: ['./store-index.component.css']
})
export class StoreIndexComponent implements OnInit {
  storeIndex: EntityIndex<Store>;
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
    this.storeService.index().subscribe((storeIndex: EntityIndex<Store>) => {
      this.storeIndex = new EntityIndex<Store>(storeIndex);
    })
  }
}
