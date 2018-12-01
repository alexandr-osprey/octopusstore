import { Component, OnInit, Input } from '@angular/core';
import { CharacteristicValueService } from 'src/app/services/characteristic-value.service';
import { ParameterService } from 'src/app/services/parameter.service';
import { CharacteristicValue } from 'src/app/view-models/characteristic-value/characteristic-value';
import { ParameterNames } from 'src/app/services/parameter-names';

@Component({
  selector: 'app-characteristic-value-index',
  templateUrl: './characteristic-value-index.component.html',
  styleUrls: ['./characteristic-value-index.component.css']
})
export class CharacteristicValueIndexComponent implements OnInit {
  protected characteristicId: number = 0;
  protected characteristicValues: CharacteristicValue[] = [];
  @Input() administrating: boolean;


  constructor(private characteristicValueService: CharacteristicValueService, private parameterService: ParameterService) { }

  ngOnInit() {
    this.parameterService.params$.subscribe(_ => {
      this.initializeComponent();
    });
    this.initializeComponent();
  }

  initializeComponent() {
    let characteristicId = +this.parameterService.getParam(ParameterNames.characteristicId);
    if (characteristicId != this.characteristicId) {
      this.characteristicId = characteristicId;
      this.characteristicValueService.index({ characteristicId: characteristicId }).subscribe(data => {
        if (data) {
          this.characteristicValues = [];
          data.entities.forEach(c => {
            this.characteristicValues.push(new CharacteristicValue(c));
          });
        }
      });
    }
  }
}
