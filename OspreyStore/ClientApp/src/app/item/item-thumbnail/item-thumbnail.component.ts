import { Component, OnInit, Input } from '@angular/core';
import { ParameterNames } from '../../parameter/parameter-names';
import { ItemThumbnail } from '../item-thumbnail';
import { ItemService } from '../item.service';
import { trigger, style, state, animate, transition } from '@angular/animations';
import { ItemImage } from 'src/app/item-image/item-image';
import { setTimeout } from 'timers';

@Component({
  selector: 'app-item-thumbnail',
  templateUrl: './item-thumbnail.component.html',
  styleUrls: ['./item-thumbnail.component.css'],
  animations: [
    trigger('expandCollapse', [
      state('expanded', style({ height: '*', opacity: 1, display: 'block' })),
      state('collapsed', style({ height: 0, opacity: 0, display: 'none' })),
      transition('expanded => collapsed', [animate('1150ms linear')]),
      transition('collapsed => expanded', [animate('1650ms linear')]),
      //transition('collapsed => expanded', [animate('350ms linear')]),
    ]),
  ],
})
export class ItemThumbnailComponent implements OnInit {
  @Input() itemThumbnail: ItemThumbnail;
  currentImage: ItemImage;
  currentImageSlideshowId: number;

  constructor(private itemService: ItemService) {
  }

  ngOnInit() {
    if (this.itemThumbnail) {
      this.setMinMaxPrices(this.itemThumbnail);
      this.itemThumbnail.images[0].shown = true;
      this.currentImage = this.itemThumbnail.images[0];
    }
  }

  getDetailUrl(itemId: number): string {
    //let str = this.itemService.getUrlWithIdWithSuffix(itemId, ParameterNames.detail);
    return this.itemService.getUrlWithIdWithSuffix(itemId, ParameterNames.detail, '/items/');
  }

  setMinMaxPrices(itemThumbnail: ItemThumbnail) {
    itemThumbnail.minPrice = Math.max(Math.min(...itemThumbnail.prices), 0);
    itemThumbnail.maxPrice = Math.max(Math.max(...itemThumbnail.prices), 0);
  }

  launchSlideShow(timestamp: number, isRecursive: boolean = false) {
    if (!isRecursive && !this.currentImageSlideshowId) {
      this.currentImageSlideshowId = timestamp;
    }
    if (this.currentImageSlideshowId != timestamp)
      return;

    let delay = 1500;

    this.itemService.delay(delay).then(() => {
      if (this.currentImageSlideshowId) {
        let nextImageIndex = this.itemThumbnail.images.indexOf(this.currentImage) + 1;
        if (nextImageIndex >= this.itemThumbnail.images.length) {
          nextImageIndex = 0;
        }
        this.currentImage = this.itemThumbnail.images[nextImageIndex];
      }
      this.launchSlideShow(timestamp, true);
    });
      

    
  }

  stopSlideShow() {
    this.currentImageSlideshowId = 0;
  }

  getTimestamp() {
    return Date.now();
  }
}
