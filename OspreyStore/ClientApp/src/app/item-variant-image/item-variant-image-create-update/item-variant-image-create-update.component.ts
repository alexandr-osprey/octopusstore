import { Component, OnInit, Input, ViewChild } from '@angular/core';
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
export class ItemVariantImageCreateUpdateComponent implements OnInit {
  itemVariantImageIndexIndexSubscription: Subscription;
  itemVariantImageSubscription: Subscription;
  itemVariantImageFormData: FormData;
  itemVariantImages: ItemVariantImage[];
  @Input() itemVariant: ItemVariant;


  constructor(
    private itemVariantImageService: ItemVariantImageService,
    private messageService: MessageService) {
    this.itemVariantImages = [];
  }

  ngOnInit() {
    this.initializeComponent()
  }

  initializeComponent() {
    if (this.itemVariant.id) {
      this.itemVariantImageService.index({ itemId: this.itemVariant.id }).subscribe((itemVariantImageIndex: EntityIndex<ItemVariantImage>) => {
        itemVariantImageIndex.entities.forEach(i => this.itemVariantImages.push(new ItemVariantImage(i)));
      });
    }
  }
  saveItemVariantImage() {
    this.itemVariantImageService.postFile(this.itemVariantImageFormData, this.itemVariant.id).subscribe((data: ItemVariantImage) => {
      if (data) {
        this.itemVariantImages.push(new ItemVariantImage(data));
        this.messageService.sendSuccess("Item variant image saved");
      }
    });
  }
  updateItemVariantImage(itemVariantImage: ItemVariantImage) {
    let i = this.itemVariantImages.indexOf(itemVariantImage);
    this.itemVariantImageService.postOrPut(itemVariantImage).subscribe((data: ItemVariantImage) => {
      if (data) {
        this.itemVariantImages[i] = new ItemVariantImage(data);
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
          this.itemVariantImages = this.itemVariantImages.filter(i => i != itemVariantImage);
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
