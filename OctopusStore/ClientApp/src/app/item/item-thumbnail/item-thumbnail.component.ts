import { Component, OnInit, Input } from '@angular/core';
import { ItemImageService } from '../../services/item-image-service';
import { ItemThumbnail } from '../../view-models/item/item-thumbnail';
import { ParameterNames } from '../../services/parameter-names';

@Component({
  selector: 'app-item-thumbnail',
  templateUrl: './item-thumbnail.component.html',
  styleUrls: ['./item-thumbnail.component.css']
})
export class ItemThumbnailComponent implements OnInit {
  @Input() itemThumbnail: ItemThumbnail;
  constructor(private itemImageService: ItemImageService) { }

  ngOnInit() {
    if (this.itemThumbnail) {
      this.setMinMaxPrices(this.itemThumbnail);
    }
  }

  getDetailsUrl(itemId: number): string {
    return `${itemId}/${ParameterNames.details}`;
  }
  setMinMaxPrices(itemThumbnail: ItemThumbnail) {
    itemThumbnail.minPrice = Math.max(Math.min(...itemThumbnail.prices), 0);
    itemThumbnail.maxPrice = Math.max(Math.max(...itemThumbnail.prices), 0);
  }
}
