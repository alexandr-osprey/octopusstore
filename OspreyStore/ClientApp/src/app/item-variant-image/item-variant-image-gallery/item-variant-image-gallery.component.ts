import { Component, OnInit, Input } from '@angular/core';
import { ItemVariantImage } from '../item-variant-image';
import { ItemVariantImageService } from '../item-variant-image.service';

@Component({
  selector: 'app-item-variant-image-gallery',
  templateUrl: './item-variant-image-gallery.component.html',
  styleUrls: ['./item-variant-image-gallery.component.css']
})
export class ItemVariantImageGalleryComponent implements OnInit {
  public displayedImage: ItemVariantImage;
  public tempDisplayedImage: ItemVariantImage;
  public hoveredThumbnail: ItemVariantImage;
  @Input() images: ItemVariantImage[];

  constructor(private itemImageService: ItemVariantImageService) { }

  ngOnInit() {
    this.initializeComponent();
  }
  initializeComponent() {
    if (this.images && this.images[0] != null) {
      this.displayedImage = this.images[0];
    }
  }
  getImageUrl(imageId: number): string {
    return this.itemImageService.getImageUrl(imageId);
  }
  thumbnailHover(thumbnailImage: ItemVariantImage) {
    this.tempDisplayedImage = this.displayedImage;
    this.displayedImage = thumbnailImage;
    this.hoveredThumbnail = thumbnailImage;
  }
  thumbnailLeave() {
    this.displayedImage = this.tempDisplayedImage;
    this.hoveredThumbnail = null;
  }
  thumbnailClick(thumbnailImage: ItemVariantImage) {
    this.displayedImage = thumbnailImage;
    this.tempDisplayedImage = this.displayedImage;
  }
}
