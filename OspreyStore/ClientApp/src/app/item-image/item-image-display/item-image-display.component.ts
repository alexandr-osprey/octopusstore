import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { ItemImage } from '../item-image';
import { ItemImageService } from '../item-image.service';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-item-image-display',
  templateUrl: './item-image-display.component.html',
  styleUrls: ['./item-image-display.component.css'],
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
export class ItemImageDisplayComponent implements OnInit, OnChanges {

  @Input() itemImage: ItemImage;
  itemImageUrl: string;

  constructor(private itemImageService: ItemImageService) {
  }

  ngOnInit() {
  }

  ngOnChanges() {
    console.log('image enter ' + this.itemImage.id);
    this.initializeComponent();
  }

  initializeComponent() {
    this.itemImageUrl = this.itemImageService.getImageUrl(this.itemImage.id);
  }
}
