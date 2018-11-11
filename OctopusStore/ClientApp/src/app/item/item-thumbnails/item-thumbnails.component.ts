import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { ParameterService } from '../../services/parameter.service';
import { debounceTime } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { EntityIndex } from '../../view-models/entity/entity-index';
import { ItemThumbnail } from '../../view-models/item/item-thumbnail';

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

  getItems(): void {
    this.itemService.indexItemThumbnails().subscribe((data: EntityIndex<ItemThumbnail>) => {
      this.itemThumbnailIndex = data;
    }
    );
  }
  ngOnDestroy() {
    // prevent memory leak when component destroyed
    this.parametersSubsription.unsubscribe();
  }
}
