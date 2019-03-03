import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { Category } from '../category';
import { CategoryService } from '../category.service';
import { MessageService } from 'src/app/message/message.service';

@Component({
  selector: 'app-category-create-update',
  templateUrl: './category-create-update.component.html',
  styleUrls: ['./category-create-update.component.css']
})
export class CategoryCreateUpdateComponent implements OnInit {
  protected category: Category;
  protected allowedCategories: Category[] = [];
  public isUpdating = false;
  @Output() categorySaved = new EventEmitter<Category>();

  constructor(private route: ActivatedRoute,
    private parameterService: ParameterService,
    private categoryService: CategoryService,
    private messageService: MessageService,
    private router: Router,
    private location: Location) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.updateAllowedCategories();
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id != 0) {
      this.isUpdating = true;
      this.categoryService.get(id).subscribe(data => {
        if (data) {
          this.category = new Category(data);
        }
      });
    } else {
      this.category = new Category();
      this.isUpdating = false;
    }
  }

  updateAllowedCategories() {
    this.categoryService.index().subscribe(data => {
      if (data) {
        this.allowedCategories = [];
        data.entities.forEach(c => {
          if (c.parentCategoryId == this.categoryService.rootCategory.id || c.id == this.categoryService.rootCategory.id) {
            this.allowedCategories.push(new Category(c));
          }
        });
        if (!this.isUpdating) {
          let parentCategoryId = +this.parameterService.getParam(ParameterNames.categoryId);
          if (this.allowedCategories.find(c => c.id == parentCategoryId)) {
            this.category.parentCategoryId = parentCategoryId;
          }
        }
      }
    });
  }

  createOrUpdate() {
    this.categoryService.postOrPut(this.category).subscribe(
      (data) => {
        if (data) {
          this.category = new Category(data);
          this.messageService.sendSuccess("Category saved");
          this.categorySaved.emit(this.category);
          if (this.categorySaved.observers.length == 0) {
            this.location.back();
          }
        }
      });
  }

}
