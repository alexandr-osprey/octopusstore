import { Component, OnInit } from '@angular/core';
import { CategoryHierarchy } from './category-hierarchy';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { Category } from '../../view-models/category/category';
import { ParameterNames } from '../../services/parameter-names';
import { CategoryService } from '../../services/category.service';
import { ParameterService } from '../../services/parameter.service';
import { EntityIndex } from '../../view-models/entity/entity-index';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css'],
})
export class CategoriesComponent implements OnInit {
  public allCategories: Category[];
  protected categoryHierarchy: CategoryHierarchy;
  protected currentCategory: Category;
  protected parametersSubsription: Subscription;
  protected categoryIdParamName = ParameterNames.categoryId;

  constructor(
    private categoryService: CategoryService,
    private parameterService: ParameterService) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.parametersSubsription = this.parameterService.params$.pipe(
      debounceTime(50))
      .subscribe(
      params => {
        this.updateCategories();
      }
    );
    this.updateCategories();
  }
  updateCategories() {
    this.allCategories = [];
    let categoryId = this.parameterService.getParam(ParameterNames.categoryId);
    if (!categoryId)
      categoryId = this.categoryService.rootCategoryId;
    this.categoryService.index(this.parameterService.getParams()).subscribe((data: EntityIndex<Category>) => {
      data.entities.forEach(c => this.allCategories.push(new Category(c)));
      this.currentCategory = this.allCategories.find(c => c.id == categoryId);
      this.categoryHierarchy = new CategoryHierarchy(this.categoryService.rootCategoryId, this.currentCategory, this.allCategories);
    });
  }
  getCategoryParams(categoryId: number): any {
    let params = this.parameterService.getUpdatedParams([ParameterNames.categoryId, categoryId]);
    params[ParameterNames.page] = 1;
    params[ParameterNames.characteristicsFilter] = ';';
    return params;
  }
}
