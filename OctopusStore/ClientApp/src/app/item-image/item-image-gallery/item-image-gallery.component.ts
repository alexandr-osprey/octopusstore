import { Component, OnInit, Input } from '@angular/core';
import { ItemImageService } from '../../services/item-image.service';
import { ItemImage } from '../../view-models/item-image/item-image';

@Component({
  selector: 'app-item-image-gallery',
  templateUrl: './item-image-gallery.component.html',
  styleUrls: ['./item-image-gallery.component.css']
})
export class ItemImageGalleryComponent implements OnInit {
  public displayedImage: ItemImage;
  public tempDisplayedImage: ItemImage;
  public hoveredThumbnail: ItemImage;
  @Input() images: ItemImage[];

  constructor(private itemImageService: ItemImageService) { }

  ngOnInit() {
    this.initializeComponent();
  }
  initializeComponent() {
    if (this.images[0] != null) {
      this.displayedImage = this.images[0];
    }
  }
  getImageUrl(imageId: number): string {
    return this.itemImageService.getImageUrl(imageId);
  }
  thumbnailHover(thumbnailImage: ItemImage) {
    this.tempDisplayedImage = this.displayedImage;
    this.displayedImage = thumbnailImage;
    this.hoveredThumbnail = thumbnailImage;
  }
  thumbnailLeave() {
    this.displayedImage = this.tempDisplayedImage;
    this.hoveredThumbnail = null;
  }
  thumbnailClick(thumbnailImage: ItemImage) {
    this.displayedImage = thumbnailImage;
    this.tempDisplayedImage = this.displayedImage;
  }
}
