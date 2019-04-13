import { Component, OnInit, Input } from '@angular/core';
import { ItemVariantImage } from '../item-variant-image';
import { ItemVariantImageService } from '../item-variant-image.service';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';

@Component({
  selector: 'app-item-variant-image-gallery',
  templateUrl: './item-variant-image-gallery.component.html',
  styleUrls: ['./item-variant-image-gallery.component.css']
})
export class ItemVariantImageGalleryComponent implements OnInit {
  public displayedImage: ItemVariantImage;
  public tempDisplayedImage: ItemVariantImage;
  public hoveredThumbnail: ItemVariantImage;
  images: ItemVariantImage[] = [];

  constructor(
    private itemVariantImageService: ItemVariantImageService,
    private parameterService: ParameterService) { }

  ngOnInit() {
    this.parameterService.params$.subscribe(p => {
      if (this.parameterService.isParamChanged("itemVariantId")) {
        this.downloadCurrentItemVariantImages();
      }
    });
    this.downloadCurrentItemVariantImages();
  }

  downloadCurrentItemVariantImages() {
    let itemVariantId = +this.parameterService.getParam(ParameterNames.itemVariantId);
    if (!itemVariantId)
      return;
    this.itemVariantImageService.index({ "itemVariantId": itemVariantId }).subscribe((data) => {
      if (data) {
        this.images = data.entities.map(i => new ItemVariantImage(i));
        this.displayedImage = this.images[0];
      }
    });
  }

  getImageUrl(imageId: number): string {
    return this.itemVariantImageService.getImageUrl(imageId);
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
