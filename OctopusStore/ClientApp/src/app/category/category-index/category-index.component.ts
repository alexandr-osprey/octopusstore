import { Component, OnInit, Input } from '@angular/core';
import { CategoryHierarchy } from './category-hierarchy';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ParameterNames } from '../../parameter/parameter-names';
import { ParameterService } from '../../parameter/parameter.service';
import { Router } from '@angular/router';
import { Category } from '../category';
import { CategoryService } from '../category.service';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { CategoryDisplayed } from '../category-displayed';

@Component({
  selector: 'app-category-index',
  templateUrl: './category-index.component.html',
  styleUrls: ['./category-index.component.css'],
})
export class CategoryIndexComponent implements OnInit {
  public allCategories: CategoryDisplayed[];
  //protected categoryHierarchy: CategoryHierarchy;
  //protected currentCategory: Category;
  //protected parametersSubsription: Subscription;
  //protected categoryIdParamName = ParameterNames.categoryId;
  //@Input() administrating: boolean;

  constructor(
    private categoryService: CategoryService,
    private router: Router,
    private parameterService: ParameterService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    //this.parametersSubsription = this.parameterService.params$.pipe(
    //  debounceTime(50))
    //  .subscribe(
    //  params => {
    //    this.updateCategories();
    //  }
    //);
    this.updateCategories();
  }
  updateCategories() {
    this.allCategories = [];
    this.categoryService.index().subscribe((data: EntityIndex<Category>) => {
      this.allCategories = data.entities
        .filter(c => c.parentCategoryId == this.categoryService.rootCategoryId)
        .map(c => {
          let nc = new CategoryDisplayed(c);
          nc.subcategories = data.entities.filter(sc => sc.parentCategoryId == c.id).map(sc => new Category(sc))
          return nc;
        });
    });
  }
  getCategoryParams(categoryId: number): any {
    let params = this.parameterService.getUpdatedParams(
      [ParameterNames.categoryId, categoryId],
      [ParameterNames.characteristicsFilter, null],
      [ParameterNames.page, null],
      [ParameterNames.characteristicId, null]);
    return params;
  }

  navigateParentCategory(categoryDisplayed: CategoryDisplayed) {
    let oldState = categoryDisplayed.collapsed;
    this.allCategories.forEach(c => c.collapsed = true);
    categoryDisplayed.collapsed = !oldState;
    let parameters = this.getCategoryParams(categoryDisplayed.id);
    this.parameterService.navigateWithUpdatedParams(parameters);
  }

  navigateSubcategory(subcategory: Category) {
    let parameters = this.getCategoryParams(subcategory.id);
    this.parameterService.navigateWithUpdatedParams(parameters);
  }
  //create() {
  //  let categoryId = this.parameterService.getParam(ParameterNames.categoryId);
  //  this.router.navigate(['/categories/create'], { queryParams: { categoryId: categoryId } });
  //}
}
