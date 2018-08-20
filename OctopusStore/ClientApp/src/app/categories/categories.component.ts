import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Category } from '../view-models/category/category';
import { CategoryService } from '../services/category.service';
import { CategoryIndex } from '../view-models/category/category-index';
import { Router, NavigationEnd } from '@angular/router';
import { CategoryDisplayed } from './category-displayed';
import { ItemService } from '../services/item.service';
import { ParameterService } from '../services/parameter-service';
import { ParameterNames } from '../services/parameter-names';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css'],
  //providers: [ParameterService]
})
export class CategoriesComponent implements OnInit {

  //@Output() paramsUpdatedEvent = new EventEmitter<any>();
  private currentCategory: CategoryDisplayed;
  categoryIdParamName = ParameterNames.categoryId;

  constructor(
    private categoryService: CategoryService,
    private parameterService: ParameterService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let params = this.parameterService.getParams();
    let categoryId = params["categoryId"] ? +params["categoryId"] : CategoryService.rootCategoryId;
    this.categoryService.index(this.getParamsForQuery()).subscribe((data: CategoryIndex) => {
      CategoryDisplayed.allCategories = data.entities;
      CategoryDisplayed.rootCategoryId = CategoryService.rootCategoryId;
      this.currentCategory = new CategoryDisplayed(data.entities.find(c => c.id == categoryId));
    });
    
  }
  getUpdatedParams(paramName: string, param: number): any {
    let params = this.parameterService.getUpdatedParams(paramName, param);
    params["page"] = 1;
    return params;
  }
  getParamsForQuery(): any {
    let params = {};
    params[ParameterNames.storeId] = this.parameterService.getParam(ParameterNames.storeId);
    return params;
  }

  categoryClick(category: Category) {
    this.currentCategory = new CategoryDisplayed(category);
    this.parameterService.setParam(ParameterNames.categoryId, category.id);
  }
}
