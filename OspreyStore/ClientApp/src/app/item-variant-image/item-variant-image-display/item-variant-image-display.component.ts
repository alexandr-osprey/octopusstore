import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import { ItemVariantImage } from '../item-variant-image';
import { ItemVariantImageService } from '../item-variant-image.service';

@Component({
  selector: 'app-item-variant-image-display',
  templateUrl: './item-variant-image-display.component.html',
  styleUrls: ['./item-variant-image-display.component.css'],
  animations: [
    trigger(
      'fadeInOut',
      [
        transition(
          ':enter', [
            style({ opacity: 0.0, display: 'none', }),
            animate('300ms linear')
          ]
        ),
        transition(
          ':leave', [
            style({ 'opacity': 1, display: 'block' }),
            animate('0ms linear')
          ]
        )
      ])
  ]
})
export class ItemVariantImageDisplayComponent implements OnInit, OnChanges {

  @Input() itemVariantImage: ItemVariantImage;
  itemVariantImageUrl: string;

  constructor(private itemVariantImageService: ItemVariantImageService) {
  }

  ngOnInit() {
  }

  ngOnChanges() {
    this.initializeComponent();
  }

  initializeComponent() {
    if (this.itemVariantImage) {
      this.itemVariantImageUrl = this.itemVariantImageService.getImageUrl(this.itemVariantImage.id);
      console.log('image enter ' + this.itemVariantImage.id);
    }
  }
}
