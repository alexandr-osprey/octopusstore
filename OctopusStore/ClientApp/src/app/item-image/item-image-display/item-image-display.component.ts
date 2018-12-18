import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { ItemImage } from '../item-image';
import { ItemImageService } from '../item-image.service';

@Component({
  selector: 'app-item-image-display',
  templateUrl: './item-image-display.component.html',
  styleUrls: ['./item-image-display.component.css']
})
export class ItemImageDisplayComponent implements OnInit, OnChanges {

  @Input() itemImage: ItemImage;
  itemImageUrl: string;

  constructor(private itemImageService: ItemImageService) {
  }

  ngOnInit() {
  }

  ngOnChanges() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.itemImageUrl = this.itemImageService.getImageUrl(this.itemImage.id);
  }
}
