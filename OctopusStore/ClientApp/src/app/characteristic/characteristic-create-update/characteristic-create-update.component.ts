import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CharacteristicService } from 'src/app/services/characteristic.service';
import { Characteristic } from 'src/app/view-models/characteristic/characteristic';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/view-models/category/category';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-characteristic-create-update',
  templateUrl: './characteristic-create-update.component.html',
  styleUrls: ['./characteristic-create-update.component.css']
})
export class CharacteristicCreateUpdateComponent implements OnInit {
  protected characteristic: Characteristic;
  protected categories: Category[] = [];
  public isUpdating = false;
  @Output() characteristicSaved = new EventEmitter<Characteristic>();

  constructor(
    private characteristicService: CharacteristicService,
    private route: ActivatedRoute,
    private messageService: MessageService,
    private categoryService: CategoryService,
    private location: Location) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.updateCategories();
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id != 0) {
      this.isUpdating = true;
      this.characteristicService.get(id).subscribe(data => {
        if (data) {
          this.characteristic = new Characteristic(data);
        }
      });
    } else {
      this.characteristic = new Characteristic();
      this.isUpdating = false;
    }
  }

  updateCategories() {
    this.categoryService.index().subscribe(data => {
      if (data) {
        this.categories = [];
        data.entities.forEach(c => {
            this.categories.push(new Category(c));
        });
      }
    });
  }

  createOrUpdate() {
    this.characteristicService.postOrPut(this.characteristic).subscribe(
      (data) => {
        if (data) {
          this.characteristic = new Category(data);
          this.messageService.sendSuccess("Characteristic saved");
          this.characteristicSaved.emit(this.characteristic);
          if (this.characteristicSaved.observers.length == 0) {
            this.location.back();
          }
        }
      });
  }

}
