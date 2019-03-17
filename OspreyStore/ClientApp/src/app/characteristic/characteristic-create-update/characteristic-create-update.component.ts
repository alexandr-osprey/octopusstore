import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { Characteristic } from '../characteristic';
import { Category } from 'src/app/category/category';
import { CharacteristicService } from '../characteristic.service';
import { MessageService } from 'src/app/message/message.service';
import { CategoryService } from 'src/app/category/category.service';

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
    private parameterService: ParameterService,
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
      let categoryId = +this.parameterService.getParam(ParameterNames.categoryId);
      this.characteristic = new Characteristic({ categoryId: categoryId });
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
          this.characteristic = new Characteristic(data);
          this.messageService.sendSuccess("Characteristic saved");
          this.characteristicSaved.emit(this.characteristic);
          if (this.characteristicSaved.observers.length == 0) {
            this.location.back();
          }
        }
      });
  }

}
