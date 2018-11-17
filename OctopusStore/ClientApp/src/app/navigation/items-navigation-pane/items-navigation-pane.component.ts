import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../services/category.service';
import { CharacteristicService } from '../../services/characteristic.service';
import { ParameterService } from '../../services/parameter.service';
import { ParameterNames } from '../../services/parameter-names';
import { Characteristic } from '../../view-models/characteristic/characteristic';

@Component({
  selector: 'app-items-navigation-pane',
  templateUrl: './items-navigation-pane.component.html',
  styleUrls: ['./items-navigation-pane.component.css']
})
export class ItemsNavigationPaneComponent implements OnInit {
  protected categoryId: number;
  protected characteristics: Characteristic[] = [];

  constructor(private categoryService: CategoryService,
    private characteristicService: CharacteristicService,
    private parameterService: ParameterService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.parameterService.params$.subscribe(params => {
      this.getCharacteristics();
    });
    this.getCharacteristics();
  }

  getCharacteristics() {
    let newCategoryId: number = +this.parameterService.getParam(ParameterNames.categoryId);
    if (newCategoryId == this.categoryId)
      return;
    if (!newCategoryId)
      this.categoryId = this.categoryService.rootCategoryId;
    this.characteristicService.index({ categoryId: this.categoryId }).subscribe(data => {
      if (data) {
        this.categoryId = newCategoryId;
        data.entities.forEach(e => this.characteristics.push(new Characteristic(e)));
      }
    });
  }
}
