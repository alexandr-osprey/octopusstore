import { Component, OnInit, Input } from '@angular/core';
import { ItemThumbnail } from '../../view-models/item/item-thumbnail';
import { ParameterNames } from '../../services/parameter-names';
import { ItemService } from '../../services/item.service';

@Component({
  selector: 'app-item-thumbnail',
  templateUrl: './item-thumbnail.component.html',
  styleUrls: ['./item-thumbnail.component.css']
})
export class ItemThumbnailComponent implements OnInit {
  @Input() itemThumbnail: ItemThumbnail;
  constructor(private itemService: ItemService) {
  }

  ngOnInit() {
    if (this.itemThumbnail) {
      this.setMinMaxPrices(this.itemThumbnail);
    }
  }

  getDetailsUrl(itemId: number): string {
    let str = this.itemService.getUrlWithIdWithSuffix(itemId, ParameterNames.details);
    return this.itemService.getUrlWithIdWithSuffix(itemId, ParameterNames.details, '/items/');
  }
  setMinMaxPrices(itemThumbnail: ItemThumbnail) {
    itemThumbnail.minPrice = Math.max(Math.min(...itemThumbnail.prices), 0);
    itemThumbnail.maxPrice = Math.max(Math.max(...itemThumbnail.prices), 0);
  }
}
