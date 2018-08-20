import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ItemThumbnailComponent } from '../item-thumbnail/item-thumbnail.component';
import { Router, NavigationEnd } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';
import { ItemService } from '../../services/item.service';
import { ItemThumbnailIndex } from '../../view-models/item/item-thumbnail-index';
import { ItemThumbnail } from '../../view-models/item/item-thumbnail';
import { ParameterService } from '../../services/parameter-service';
import { ParameterNames } from '../../services/parameter-names';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-item-thumbnails',
  templateUrl: './item-thumbnails.component.html',
  styleUrls: ['./item-thumbnails.component.css'],
  providers: [ItemService]
})
export class ItemThumbnailsComponent implements OnInit, OnDestroy {
  itemThumbnailIndex: ItemThumbnailIndex;
  parametersSubsription: Subscription;

  constructor(
    private itemService: ItemService,
    private router: Router,
    private parameterService: ParameterService)
  {
    this.parametersSubsription = parameterService.params$.pipe(
      debounceTime(100),
      distinctUntilChanged(),
    ).subscribe(
      params => {
        this.getItems();
      }
    );
  }
  error: string;
  navigationSubscription;

  initializeComponent() {
    this.getItems();
  }

  ngOnInit() {
    this.initializeComponent();
  }

  getItems(): void {
    let params = this.parameterService.getParams()
    this.itemService.indexCustom<ItemThumbnailIndex>(
      this.itemService.getUrlWithParameter(ParameterNames.thumbnails), params)
      .subscribe((data: ItemThumbnailIndex) => {
        this.itemThumbnailIndex = Object.assign(new ItemThumbnailIndex(), data);
        this.router.navigate([ParameterService.getUrlWithoutParams(this.router)], { queryParams: params });
    });
  }
  ngOnDestroy() {
    // prevent memory leak when component destroyed
    this.parametersSubsription.unsubscribe();
  }
}
