import { Component, OnInit, Input } from '@angular/core';
import { ItemImage } from '../../view-models/item-image/item-image';
import { ActivatedRoute } from '@angular/router';
import { ItemImageService } from '../../services/item-image-service';
import { ItemImageIndex } from '../../view-models/item-image/item-image-index';
import { Item } from '../../view-models/item/item';

@Component({
  selector: 'app-item-image-create-update',
  templateUrl: './item-image-create-update.component.html',
  styleUrls: ['./item-image-create-update.component.css']
})
export class ItemImageCreateUpdateComponent implements OnInit {

  itemImageFormData: FormData;
  itemImages: ItemImage[];
  @Input() item: Item;

  constructor(
    private itemImageService: ItemImageService,
    private route: ActivatedRoute) {
    this.itemImages = [];
  }

  ngOnInit() {
    this.initializeComponent()
  }

  initializeComponent() {
    if (this.item.id) {
      this.itemImageService.index({ itemId: this.item.id }).subscribe((data: ItemImageIndex) => {
        this.itemImages.push.apply(this.itemImages, data.entities);
      })
    }
  }
  saveItemImage() {
    this.itemImageService.postFormData(this.itemImageFormData, this.itemImageService.filePostUrl).subscribe((data: ItemImage) => {
      if (data)
        this.itemImages.push(data);
    });
  }
  updateItemImage(itemImage: ItemImage) {
    let i = this.itemImages.indexOf(itemImage);
    this.itemImageService.update(itemImage).subscribe((data: ItemImage) => {
      if (data)
        this.itemImages[i] = data;
    });
  }
  getImageUrl(itemImage: ItemImage): string {
    return this.itemImageService.getImageUrl(itemImage.id)
  }

  deleteItemImage(itemImage: ItemImage) {
    this.itemImageService.delete(itemImage.id).subscribe((data) => {
      if (data)
        this.itemImages = this.itemImages.filter(i => i != itemImage);
    })
  }

  onFileChanged(event) {
    let selectedFile = event.target.files[0];
    this.itemImageFormData = new FormData();
    this.itemImageFormData.append('formFile', selectedFile, `${this.item.id}`);
  }
}
