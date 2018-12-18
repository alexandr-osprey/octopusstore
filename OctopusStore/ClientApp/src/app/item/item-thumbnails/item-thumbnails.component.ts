import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ParameterService } from '../../parameter/parameter.service';
import { debounceTime } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { ParameterNames } from '../../parameter/parameter-names';
import { ItemService } from '../item.service';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { ItemThumbnail } from '../item-thumbnail';

@Component({
  selector: 'app-item-thumbnails',
  templateUrl: './item-thumbnails.component.html',
  styleUrls: ['./item-thumbnails.component.css'],
  providers: [ItemService]
})
export class ItemThumbnailsComponent implements OnInit, OnDestroy {
  itemThumbnailIndex: EntityIndex<ItemThumbnail>;
  parametersSubsription: Subscription;
  itemThumbnailsSubsription: Subscription;

  constructor(
    private itemService: ItemService,
    private parameterService: ParameterService)
  {
    this.parametersSubsription = this.parameterService.params$.pipe(
      debounceTime(10),
      //distinctUntilChanged(),
    ).subscribe(
      params => {
        this.getItems();
      }
    );
  }
  error: string;
  navigationSubscription;

  initializeComponent() {
    //this.parameterService.clearParams();
    this.getItems();
    
  }

  ngOnInit() {
    this.initializeComponent();
  }

  getOrderByPriceQueryParams(): any {
    return this.getOrderByQueryParams('price');
  }

  getOrderByTitleQueryParams(): any {
    return this.getOrderByQueryParams('title');
  }

  getOrderByQueryParams(paramName: string): any {
    let updatedParams = this.parameterService.getUpdatedParams([ParameterNames.orderBy, paramName]);
    updatedParams[ParameterNames.page] = 1;
    updatedParams[ParameterNames.orderByDescending] = this.getOrderByDescending(paramName);
    return updatedParams;
  }

  

  getOrderByDescending(paramName: string): boolean {
    let param = this.parameterService.getParam(ParameterNames.orderBy) == paramName;
    if (!param)
      return false;
    // desc is "false"
    //let desc: boolean = this.parameterService.getParam(ParameterNames.orderByDescending);
    // but orderByDesc is false! wtf
    //let orderByDesc: boolean = !desc;
    let desc: boolean = this.parameterService.getParam(ParameterNames.orderByDescending) == "true";
    let orderByDesc: boolean = !desc;
    return orderByDesc;
  }


  getItems(): void {
    this.itemService.indexItemThumbnails().subscribe((data: EntityIndex<ItemThumbnail>) => {
      this.itemThumbnailIndex = data;
    });
  }
  ngOnDestroy() {
    // prevent memory leak when component destroyed
    this.parametersSubsription.unsubscribe();
  }
}
