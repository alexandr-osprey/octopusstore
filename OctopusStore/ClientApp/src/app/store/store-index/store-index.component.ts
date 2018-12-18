import { Component, OnInit } from '@angular/core';
import { ParameterNames } from '../../parameter/parameter-names';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { Store } from '../store';
import { StoreService } from '../store.service';

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
