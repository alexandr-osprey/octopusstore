import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ParameterService } from 'src/app/services/parameter.service';
import { ParameterNames } from 'src/app/services/parameter-names';
import { Category } from 'src/app/view-models/category/category';
import { CategoryService } from 'src/app/services/category.service';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-category-create-update',
  templateUrl: './category-create-update.component.html',
  styleUrls: ['./category-create-update.component.css']
})
export class CategoryCreateUpdateComponent implements OnInit {
  protected category: Category;
  protected allCategories: Category[] = [];
  public isUpdating = false;
  @Output() categorySaved = new EventEmitter<Category>();

  constructor(private route: ActivatedRoute,
    private parameterService: ParameterService,
    private categoryService: CategoryService,
    private messageService: MessageService,
    private router: Router) {
  }

  ngOnInit() {
    this.parameterService.params$.subscribe(p => {
      this.initializeComponent();
    });
    this.initializeComponent();
  }

  initializeComponent() {
      this.updateAllCategories();
      let id = +this.route.snapshot.paramMap.get('id') || 0;
      if(id == 0) {
        id = +this.parameterService.getParam(ParameterNames.categoryId) || 0;
      }

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

  updateAllCategories() {
    this.categoryService.index().subscribe(data => {
      if (data) {
        this.allCategories = [];
        data.entities.forEach(c => {
          if (c.parentCategoryId == this.categoryService.rootCategoryId || c.id == this.categoryService.rootCategoryId) {
            this.allCategories.push(new Category(c));
          }
        });
      }
    });
  }

  createOrUpdate() {
    this.categoryService.postOrPut(this.category).subscribe(
      (data) => {
        if (data) {
          //this.messageService.sendError("subscribe 3: ");
          this.category = new Category(data);
          this.messageService.sendSuccess("Category updated");
          this.updateAllCategories();
          this.categorySaved.emit(this.category);
          if (!this.categorySaved.observers.length) {
            this.parameterService.navigateWithParams('/categories/control/');
          }
        }
      });
  }

}
