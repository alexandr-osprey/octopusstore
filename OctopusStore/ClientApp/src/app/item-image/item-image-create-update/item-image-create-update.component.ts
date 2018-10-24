import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ItemImage } from '../../view-models/item-image/item-image';
import { ActivatedRoute } from '@angular/router';
import { ItemImageService } from '../../services/item-image-service';
import { ItemImageIndex } from '../../view-models/item-image/item-image-index';
import { Item } from '../../view-models/item/item';
import { Subscription } from 'rxjs';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-item-image-create-update',
  templateUrl: './item-image-create-update.component.html',
  styleUrls: ['./item-image-create-update.component.css']
})
export class ItemImageCreateUpdateComponent implements OnInit {
  itemImageIndexIndexSubscription: Subscription;
  itemImageSubscription: Subscription;
  itemImageFormData: FormData;
  itemImages: ItemImage[];
  @Input() item: Item;


  constructor(
    private itemImageService: ItemImageService,
    private messageService: MessageService) {
    this.itemImages = [];
  }

  ngOnInit() {
    this.initializeComponent()
  }

  initializeComponent() {
    if (this.item.id) {
      this.itemImageService.index({ itemId: this.item.id }).subscribe((itemImageIndex: ItemImageIndex) => {
        itemImageIndex.entities.forEach(i => this.itemImages.push(new ItemImage(i)));
      });
    }
  }
  saveItemImage() {
    this.itemImageService.postFile(this.itemImageFormData, this.item.id).subscribe((data: ItemImage) => {
      if (data) {
        this.itemImages.push(new ItemImage(data));
        this.messageService.sendSuccess("Item image saved");
      }
    });
  }
  updateItemImage(itemImage: ItemImage) {
    let i = this.itemImages.indexOf(itemImage);
    this.itemImageService.postOrPut(itemImage).subscribe((data: ItemImage) => {
      if (data) {
        this.itemImages[i] = new ItemImage(data);
        this.messageService.sendSuccess("Item image updated");
      }
    });;
  }
  getImageUrl(itemImage: ItemImage): string {
    return this.itemImageService.getImageUrl(itemImage.id)
  }

  deleteItemImage(itemImage: ItemImage) {
    if (itemImage.id) {
      this.itemImageService.delete(itemImage.id).subscribe((data) => {
        if (data) {
          this.itemImages = this.itemImages.filter(i => i != itemImage);
          this.messageService.sendSuccess("Item image deleted");
        }
      })
    }
  }

  onFileChanged(event) {
    let selectedFile = event.target.files[0];
    this.itemImageFormData = new FormData();
    this.itemImageFormData.append('formFile', selectedFile, selectedFile.name);
  }
}
