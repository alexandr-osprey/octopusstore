import { Component, OnInit } from '@angular/core';
import { ParameterService } from '../../parameter/parameter.service';
import { ParameterNames } from '../../parameter/parameter-names';
import { Characteristic } from 'src/app/characteristic/characteristic';
import { CategoryService } from 'src/app/category/category.service';
import { CharacteristicService } from 'src/app/characteristic/characteristic.service';
import { CharacteristicValue } from 'src/app/characteristic-value/characteristic-value';
import { Category } from 'src/app/category/category';

@Component({
  selector: 'app-items-navigation-pane',
  templateUrl: './items-navigation-pane.component.html',
  styleUrls: ['./items-navigation-pane.component.css']
})
export class ItemsNavigationPaneComponent implements OnInit {
  protected categoryId: number;
  protected characteristics: Characteristic[] = [];
  protected selectedChacarteristicValueIds: number[] = [];

  constructor(private categoryService: CategoryService,
    private characteristicService: CharacteristicService,
    private parameterService: ParameterService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.parameterService.params$.subscribe(params => {
      this.getCharacteristics(false);
    });
    this.getCharacteristics(true);
  }

  getCharacteristics(init: boolean) {
    let newCategoryId: number = +this.parameterService.getParam(ParameterNames.categoryId);
    if (newCategoryId !== this.categoryId) {
      this.selectedChacarteristicValueIds = [];
    }
    else {
      return;
    }
    if (!newCategoryId)
      newCategoryId = this.categoryService.rootCategoryId;
    this.categoryId = newCategoryId;
    
    this.characteristicService.index({ categoryId: this.categoryId }).subscribe(data => {
      if (data) {
        this.characteristics = [];
        this.categoryId = newCategoryId;
        data.entities.forEach(e => this.characteristics.push(new Characteristic(e)));
        if (init) {
          let filters: string = this.parameterService.getParam(ParameterNames.characteristicsFilter);
          if (filters) {
            this.selectedChacarteristicValueIds = filters.split(';').map(v => +v);
          }
          this.applyFilter();
        }
      }
    });
  }

  characteristicValueSelected(characteristicValue: CharacteristicValue) {
    let existing = this.selectedChacarteristicValueIds.find(i => i == characteristicValue.id);
    if (!existing) {
      this.selectedChacarteristicValueIds.push(characteristicValue.id);
      this.applyFilter();
    }
  }

  characteristicValueUnselected(characteristicValue: CharacteristicValue) {
    let existing = this.selectedChacarteristicValueIds.find(i => i == characteristicValue.id);
    if (existing) {
      this.selectedChacarteristicValueIds = this.selectedChacarteristicValueIds.filter(i => i != characteristicValue.id);
      this.applyFilter();
    }
  }

  applyFilter() {
    let p = this.selectedChacarteristicValueIds.join(';');
    this.parameterService.navigateWithUpdatedParams({ "characteristicsFilter": p, "page": 1 });
  }
}
