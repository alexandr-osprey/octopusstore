import { Component, OnInit } from '@angular/core';
import { StoreIndex } from '../../view-models/store/store-index';
import { Brand } from '../../view-models/brand/brand';
import { Store } from '../../view-models/store/store';
import { Category } from '../../view-models/category/category';
import { MeasurementUnit } from '../../view-models/measurement-unit/measurement-unit';
import { BrandService } from '../../services/brand.service';
import { StoreService } from '../../services/store.service';
import { MeasurementUnitService } from '../../services/measurement-unit.service';
import { CategoryService } from '../../services/category.service';
import { ItemService } from '../../services/item.service';
import { BrandIndex } from '../../view-models/brand/brand-index';
import { MeasurementUnitIndex } from '../../view-models/measurement-unit/measurement-unit-index';
import { CategoryIndex } from '../../view-models/category/category-index';
import { Router, ActivatedRoute } from '@angular/router';
import { Item } from '../../view-models/item/item';
import { ItemVariant } from '../../view-models/item-variant/item-variant';
import 'rxjs/add/observable/zip';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-item-create-update',
  templateUrl: './item-create-update.component.html',
  styleUrls: ['./item-create-update.component.css'],
  providers: [ItemService]
})
export class ItemCreateUpdateComponent implements OnInit {

  public brands: Brand[];
  public stores: Store[];
  public allCategories: Category[];
  public measurementUnits: MeasurementUnit[];
  public parentCategories: Category[];
  public subcategories: Category[];
  public set parentCategoryId(parentCategoryId: number) {
    this._parentCategoryId = parentCategoryId;
    this.filterSubcategories(parentCategoryId)
  }
  public get parentCategoryId(): number {
    return this._parentCategoryId;
  }
  public parent
  public itemVariants: ItemVariant[];
  public item: Item;

  private _parentCategoryId: number;

  constructor(
    private brandService: BrandService,
    private storeService: StoreService,
    private measurementUnitService: MeasurementUnitService,
    private categoryService: CategoryService,
    private itemService: ItemService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let id = +this.route.snapshot.paramMap.get('id');
    Observable.zip(
      this.brandService.index(),
      this.storeService.index(),
      this.measurementUnitService.index(),
      this.categoryService.index()).subscribe((data) => {
      this.brands = data[0].entities;
      this.stores = data[1].entities;
      this.measurementUnits = data[2].entities;
      this.allCategories = data[3].entities;
      this.parentCategories = this.allCategories.filter(c => c.parentCategoryId == CategoryService.rootCategoryId);
      if (id) {
        this.itemService.get(id).subscribe((item: Item) => {
          this.item = item;
          this.parentCategoryId = this.allCategories.find(c => c.id == this.item.categoryId).parentCategoryId;
        })
      } else {
        this.item = new Item();
      };
    });
    }

  filterSubcategories(parentCategoryId: number) {
    this.subcategories = this.allCategories.filter(c => c.parentCategoryId == parentCategoryId);
  }

  createOrUpdate() {
    this.itemService.createOrUpdate(this.item).subscribe(
      (data: Item) => {
        this.item = data;
        this.router.navigate([`/items/${data.id}/update`]);
      });
  }
}
