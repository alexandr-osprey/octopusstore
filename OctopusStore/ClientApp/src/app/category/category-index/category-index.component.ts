import { Component, OnInit, Input } from '@angular/core';
import { CategoryHierarchy } from './category-hierarchy';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ParameterNames } from '../../parameter/parameter-names';
import { ParameterService } from '../../parameter/parameter.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Category } from '../category';
import { CategoryService } from '../category.service';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { CategoryDisplayed } from '../category-displayed';
import { trigger, style, state, animate, transition } from '@angular/animations';


@Component({
  selector: 'app-category-index',
  templateUrl: './category-index.component.html',
  styleUrls: ['./category-index.component.css'],
  animations: [
    trigger('expandCollapse', [
      state('expanded', style({ height: '*', opacity: 1, display: 'block' })),
      state('collapsed', style({ height: 0, opacity: 0, display: 'none' })),
      transition('expanded => collapsed', [animate('350ms linear')]),
      transition('collapsed => expanded', [animate('350ms linear')]),
      //transition('collapsed => expanded', [animate('350ms linear')]),
    ]),
  ],
})
export class CategoryIndexComponent implements OnInit {
  public allCategories: CategoryDisplayed[];
  currentCategory: Category;
  isOpen = true;
  parametersSubscription: Subscription;
  //protected categoryHierarchy: CategoryHierarchy;
  //protected currentCategory: Category;
  //protected parametersSubsription: Subscription;
  //protected categoryIdParamName = ParameterNames.categoryId;
  //@Input() administrating: boolean;

  constructor(
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private parameterService: ParameterService) {
    this.parametersSubscription = this.parameterService.params$.subscribe(params => {
      if (!this.currentCategory || this.currentCategory.id != +this.parameterService.getParam(ParameterNames.categoryId)) {
        this.updateCategories();
      };
    });
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.parameterService.params$.pipe(
      debounceTime(50))
      .subscribe(
      params => {
        this.updateCategories();
      }
    );
    this.categoryService.index().subscribe((data: EntityIndex<Category>) => {
      this.allCategories = data.entities
        .filter(c => c.parentCategoryId == this.categoryService.rootCategoryId)
        .map(c => {
          let nc = new CategoryDisplayed(c)
          nc.subcategories = data.entities.filter(sc => sc.parentCategoryId == c.id).map(sc => new Category(sc))
          return nc;
        });
      this.updateCategories();
    });
  }

  updateCategories() {
    let categoryId = +this.parameterService.getParam(ParameterNames.categoryId);
    if (categoryId) {
      let selectedCategory = this.allCategories.find(c => c.id == categoryId);
      if (selectedCategory) {
        this.setCategoryDisplayed(selectedCategory);
        this.currentCategory = selectedCategory;
      } else {
        let selectedSubcategory = this.allCategories.map(c => c.subcategories)
          .reduce((a, b) => a.concat(b))
          .find(c => c.id == categoryId);
        if (selectedSubcategory) {
          selectedCategory = this.allCategories.find(c => c.id == selectedSubcategory.parentCategoryId);
          this.setCategoryDisplayed(selectedCategory);
          if (selectedCategory) {
            selectedCategory.currentSubcategory = selectedSubcategory;
            this.currentCategory = selectedSubcategory;
          }
        }
      }
    };
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
    let parameters = this.getCategoryParams(categoryDisplayed.id);
    this.parameterService.navigateWithUpdatedParams(parameters);
  }

  setCategoryDisplayed(categoryDisplayed: CategoryDisplayed) {
    //this.allCategories.forEach(c => {
    //  if (c.expanded && c != categoryDisplayed) {
    //    c.expanded = false
    //  }

    //});
    this.allCategories.forEach(c => {
      c.expanded = false;
    });
    categoryDisplayed.expanded = true;
  }

  navigateSubcategory(category: CategoryDisplayed, subcategory: Category) {
    let parameters = this.getCategoryParams(subcategory.id);
    this.parameterService.navigateWithUpdatedParams(parameters);
  }

  getType(c: any): string {
    return c.constructor.name;
  }
  //create() {
  //  let categoryId = this.parameterService.getParam(ParameterNames.categoryId);
  //  this.router.navigate(['/categories/create'], { queryParams: { categoryId: categoryId } });
  //}

}
