import { Component, OnInit, Input, ViewChild, OnChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { MessageService } from 'src/app/message/message.service';
import { EntityIndex } from 'src/app/models/entity/entity-index';
import { ItemVariantImageService } from '../item-variant-image.service';
import { ItemVariantImage } from '../item-variant-image';
import { ItemVariant } from 'src/app/item-variant/item-variant';

@Component({
  selector: 'app-item-variant-image-create-update',
  templateUrl: './item-variant-image-create-update.component.html',
  styleUrls: ['./item-variant-image-create-update.component.css']
})
export class ItemVariantImageCreateUpdateComponent implements OnChanges {
  itemVariantImageIndexIndexSubscription: Subscription;
  itemVariantImageSubscription: Subscription;
  itemVariantImageFormData: FormData;
  allItemVariantImages = new Map<number, ItemVariantImage[]>();
  displayedImages: ItemVariantImage[] = [];
  @Input() itemVariant: ItemVariant;


  constructor(
    private itemVariantImageService: ItemVariantImageService,
    private messageService: MessageService) {
  }

  ngOnChanges() {
    this.initializeComponent()
  }

  initializeComponent() {
    if (this.itemVariant && this.itemVariant.id) {
      if (!this.allItemVariantImages.has(this.itemVariant.id)) {
        this.itemVariantImageService.index({ "itemVariantId": this.itemVariant.id }).subscribe((itemVariantImageIndex: EntityIndex<ItemVariantImage>) => {
          this.allItemVariantImages.set(this.itemVariant.id, itemVariantImageIndex.entities.map(i => new ItemVariantImage(i)));
          this.displayedImages = this.allItemVariantImages.get(this.itemVariant.id);
        });        
      }
      this.displayedImages = this.allItemVariantImages.get(this.itemVariant.id);
    }
  }
  saveitemVariantImage() {
    this.itemVariantImageService.postFile(this.itemVariantImageFormData, this.itemVariant.id).subscribe((data: ItemVariantImage) => {
      if (data) {
        this.allItemVariantImages.get(this.itemVariant.id).push(new ItemVariantImage(data));
        this.messageService.sendSuccess("Item variant image saved");
      }
    });
  }
  updateItemVariantImage(itemVariantImage: ItemVariantImage) {
    let i = this.allItemVariantImages.get(this.itemVariant.id).indexOf(itemVariantImage);
    this.itemVariantImageService.postOrPut(itemVariantImage).subscribe((data: ItemVariantImage) => {
      if (data) {
        this.allItemVariantImages.get(this.itemVariant.id)[i] = new ItemVariantImage(data);
        this.messageService.sendSuccess("Item variant image updated");
      }
    });;
  }
  getImageUrl(itemVariantImage: ItemVariantImage): string {
    return this.itemVariantImageService.getImageUrl(itemVariantImage.id)
  }

  deleteItemVariantImage(itemVariantImage: ItemVariantImage) {
    if (itemVariantImage.id) {
      this.itemVariantImageService.delete(itemVariantImage.id).subscribe((data) => {
        if (data) {
          let images = this.allItemVariantImages.get(this.itemVariant.id);
          let imagesLeft = images.filter(i => i != itemVariantImage);
          this.allItemVariantImages.set(this.itemVariant.id, imagesLeft);
          this.displayedImages = imagesLeft;
          this.messageService.sendSuccess("Item variant image deleted");
        }
      })
    }
  }

  onFileChanged(event) {
    let selectedFile = event.target.files[0];
    this.itemVariantImageFormData = new FormData();
    this.itemVariantImageFormData.append('formFile', selectedFile, selectedFile.name);
  }
}
