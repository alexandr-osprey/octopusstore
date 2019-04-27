import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import 'rxjs/add/observable/zip';
import { Observable, Subscription } from 'rxjs';
import { Brand } from 'src/app/brand/brand';
import { Store } from 'src/app/store/store';
import { Category } from 'src/app/category/category';
import { ItemVariant } from 'src/app/item-variant/item-variant';
import { Item } from '../item';
import { BrandService } from 'src/app/brand/brand.service';
import { StoreService } from 'src/app/store/store.service';
import { CategoryService } from 'src/app/category/category.service';
import { ItemService } from '../item.service';
import { MessageService } from 'src/app/message/message.service';

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
  public parentCategories: Category[];
  public subcategories: Category[];
  public isUpdating = false;
  public set parentCategoryId(parentCategoryId: number) {
    this._parentCategoryId = parentCategoryId;
    this.filterSubcategories(parentCategoryId)
  }
  public get parentCategoryId(): number {
    return this._parentCategoryId;
  }
  public itemVariants: ItemVariant[];
  public item: Item;

  private _parentCategoryId: number;

  constructor(
    private brandService: BrandService,
    private storeService: StoreService,
    private categoryService: CategoryService,
    private itemService: ItemService,
    private router: Router,
    private route: ActivatedRoute,
    private messageService: MessageService
  ) {
  }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.brands = [];
    this.stores = [];
    this.allCategories = [];
    this.parentCategories = [];
    //this.messageService.sendError("Init: ");
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id)
      this.isUpdating = true;
    let observables: Observable<any>;
    observables = Observable.zip(
      this.brandService.index(),
      this.storeService.index({ updateAuthorizationFilter: true}),
        this.categoryService.index()
      );
    observables.subscribe((data) => {
      //this.messageService.sendError("subscribe 1: ");
      data[0].entities.forEach(b => this.brands.push(new Brand(b)));
      data[1].entities.forEach(s => this.stores.push(new Store(s)));
      data[2].entities.forEach(c => this.allCategories.push(new Category(c)));
      this.parentCategories = this.allCategories.filter(c => c.parentCategoryId == this.categoryService.rootCategory.id);
      if (id) {
        this.itemService.get(id).subscribe((item: Item) => {
          if (item) {
            //this.messageService.sendError("subscribe 2: ");
            this.item = new Item(item);
            this.parentCategoryId = this.allCategories.find(c => c.id == this.item.categoryId).parentCategoryId;
          } 
        });
      } else {
        this.item = new Item();
      };
    });

    //localStorage.removeItem('item-create-update-component-help-shown');
    if (!localStorage.getItem('item-create-update-component-help-shown')) {
      this.showHelpMessages();
      localStorage.setItem('item-create-update-component-help-shown', 'true');
    }
  }

  showHelpMessages() {
    this.itemService.delay(2 * 1000).then(() =>
      this.messageService.sendHelp("Store owner is able to edit and create item and it's variants, upload images. Also available for webiste administrators. "));
  }

  filterSubcategories(parentCategoryId: number) {
    this.subcategories = this.allCategories.filter(c => c.parentCategoryId == parentCategoryId);
  }

  createOrUpdate() {
    this.itemService.postOrPut(this.item).subscribe(
      (data: Item) => {
        if (data) {
          //this.messageService.sendError("subscribe 3: ");
          this.item = new Item(data);
          this.router.navigate([`/items/${data.id}/update`]);
          this.messageService.sendSuccess("Item updated");
        }
      });
  }
  delete() {
    if (this.item.id) {
      this.itemService.delete(this.item.id)
        .subscribe(data => {
          if (data) {
            this.messageService.sendSuccess("Item deleted");
            this.router.navigate([`/items/`]);
          }
        });
    }
  }
}
