import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { ItemThumbnailIndex } from '../../view-models/item/item-thumbnail-index';
import { ParameterService } from '../../services/parameter-service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-item-thumbnails',
  templateUrl: './item-thumbnails.component.html',
  styleUrls: ['./item-thumbnails.component.css'],
  providers: [ItemService]
})
export class ItemThumbnailsComponent implements OnInit, OnDestroy {
  itemThumbnailIndex: ItemThumbnailIndex;
  parametersSubsription: Subscription;
  itemThumbnailsSubsription: Subscription;

  constructor(
    private itemService: ItemService,
    private parameterService: ParameterService)
  {
    this.parametersSubsription = this.parameterService.params$.pipe(
      debounceTime(50),
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
    this.itemService.indexItemThumbnails().subscribe(data => {
      this.itemThumbnailIndex = data;
    }
    );
  }
  ngOnDestroy() {
    // prevent memory leak when component destroyed
    this.parametersSubsription.unsubscribe();
  }
}
