import { Component, OnInit, Input } from '@angular/core';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { Router } from '@angular/router';
import { Characteristic } from '../characteristic';
import { CharacteristicService } from '../characteristic.service';

@Component({
  selector: 'app-characteristic-index',
  templateUrl: './characteristic-index.component.html',
  styleUrls: ['./characteristic-index.component.css']
})
export class CharacteristicIndexComponent implements OnInit {
  protected characteristics: Characteristic[] = [];
  protected categoryId: number = 0;
  @Input() administrating: boolean;

  constructor(
    private parameterService: ParameterService,
    private characteristicService: CharacteristicService,
    private router: Router) { }

  ngOnInit() {
    this.parameterService.params$.subscribe(_ => {
      this.initializeComponent();
    });
    this.initializeComponent();
  }

  initializeComponent() {
    let categoryId = +this.parameterService.getParam(ParameterNames.categoryId);
    if (categoryId != this.categoryId) {
      this.categoryId = categoryId;
      this.characteristicService.index({ categoryId: categoryId }).subscribe(data => {
        if (data) {
          this.characteristics = [];
          data.entities.forEach(c => {
            this.characteristics.push(new Characteristic(c));
          });
        }
      });
    }
  }

  getCharacteristicParams(characteristic: Characteristic): any {
    let characteristicParams = this.parameterService.getUpdatedParamsCopy(
      { "characteristicId": characteristic.id, });
    return characteristicParams;
  }

  create() {
    let categoryId = this.parameterService.getParam(ParameterNames.categoryId);
    this.router.navigate(['/characteristics/create'], { queryParams: { categoryId: categoryId } });
  }
}
