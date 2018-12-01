import { Component, OnInit, Input } from '@angular/core';
import { ParameterService } from 'src/app/services/parameter.service';
import { CharacteristicService } from 'src/app/services/characteristic.service';
import { Characteristic } from 'src/app/view-models/characteristic/characteristic';
import { ParameterNames } from 'src/app/services/parameter-names';

@Component({
  selector: 'app-characteristic-index',
  templateUrl: './characteristic-index.component.html',
  styleUrls: ['./characteristic-index.component.css']
})
export class CharacteristicIndexComponent implements OnInit {
  protected characteristics: Characteristic[] = [];
  protected categoryId: number = 0;
  @Input() administrating: boolean;

  constructor(private parameterService: ParameterService, private characteristicService: CharacteristicService) { }

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
    let characteristicParams = this.parameterService.getUpdatedParams(
      [ParameterNames.characteristicId, characteristic.id]);
    return characteristicParams;
  }
}
