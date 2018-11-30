import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../services/category.service';
import { CharacteristicService } from '../../services/characteristic.service';
import { ParameterService } from '../../services/parameter.service';
import { ParameterNames } from '../../services/parameter-names';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { CharacteristicValue } from 'src/app/view-models/characteristic-value/characteristic-value';

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
    if (newCategoryId == this.categoryId)
      return;
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
    this.parameterService.navigateWithUpdatedParams([ParameterNames.characteristicsFilter, p], [ParameterNames.page, 1]);
  }
}
