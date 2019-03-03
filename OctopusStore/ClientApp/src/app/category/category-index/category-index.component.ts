import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { debounceTime } from 'rxjs/operators';
import { ParameterNames } from '../../parameter/parameter-names';
import { ParameterService } from '../../parameter/parameter.service';
import { ActivatedRoute } from '@angular/router';
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
  public rootCategory: CategoryDisplayed;
  @Output() categorySelected = new EventEmitter<Category>();
  //protected categoryHierarchy: CategoryHierarchy;
  //protected currentCategory: Category;
  //protected parametersSubsription: Subscription;
  //protected categoryIdParamName = ParameterNames.categoryId;
  //@Input() administrating: boolean;

  constructor(
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private parameterService: ParameterService) {
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
      this.rootCategory = new CategoryDisplayed({ id: this.categoryService.rootCategory.id, title: 'Items' })
      this.allCategories = data.entities
        .filter(c => c.parentCategoryId == this.rootCategory.id)
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
      if (categoryId == this.rootCategory.id) {
        this.setCategoryDisplayed(this.rootCategory);
        return;
      };
      let selectedCategory = this.allCategories.find(c => c.id == categoryId);
      if (selectedCategory) {
        this.setCategoryDisplayed(selectedCategory);
      } else {
        let selectedSubcategory = this.allCategories.map(c => c.subcategories)
          .reduce((a, b) => a.concat(b))
          .find(c => c.id == categoryId);
        if (selectedSubcategory) {
          selectedCategory = this.allCategories.find(c => c.id == selectedSubcategory.parentCategoryId);
          this.setCategoryDisplayed(selectedCategory);
          if (selectedCategory) {
            selectedCategory.currentSubcategory = selectedSubcategory;
          }
        }
      }
    };
  }
  getCategoryParams(categoryId: number): any {
    let params = this.parameterService.getUpdatedParams({
      "categoryId": categoryId,
      "characteristicsFilter": null,
      "page": null,
      "characteristicId": null
    });
    return params;
  }

  navigateParentCategory(categoryDisplayed: CategoryDisplayed) {
    let parameters = this.getCategoryParams(categoryDisplayed.id);
    this.parameterService.navigateWithUpdatedParams(parameters);
    this.categorySelected.emit(categoryDisplayed);
  }

  navigateRootCategory() {
    let parameters = this.getCategoryParams(this.categoryService.rootCategory.id);
    this.parameterService.navigateWithParams(parameters);
    this.categorySelected.emit(this.categoryService.rootCategory)
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
    this.parameterService.navigateWithParams(parameters);
    this.categorySelected.emit(subcategory);
  }
}
