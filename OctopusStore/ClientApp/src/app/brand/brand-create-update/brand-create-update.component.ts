import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BrandService } from 'src/app/services/brand.service';
import { Brand } from 'src/app/view-models/brand/brand';
import { MessageService } from 'src/app/services/message.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-brand-create-update',
  templateUrl: './brand-create-update.component.html',
  styleUrls: ['./brand-create-update.component.css']
})
export class BrandCreateUpdateComponent implements OnInit {
  isUpdating: boolean = false;
  brand: Brand;
  @Output() brandSaved = new EventEmitter<Brand>();
  
  constructor(
    private route: ActivatedRoute,
    private brandServcie: BrandService,
    private messageServcie: MessageService,
    private location: Location) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id != 0) {
      this.isUpdating = true;
      this.brandServcie.get(id).subscribe(data => {
        if (data) {
          this.brand = new Brand(data);
        }
      });
    } else {
      this.brand = new Brand();
      this.isUpdating = false;
    }
  }

  createOrUpdate() {
    this.brandServcie.postOrPut(this.brand).subscribe(
      (data) => {
        if (data) {
          this.brand = new Brand(data);
          this.messageServcie.sendSuccess("Brand saved");
          this.brandSaved.emit(this.brand);
          if (this.brandSaved.observers.length == 0) {
            this.location.back();
          }
        }
      });
  }

}
