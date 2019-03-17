import { Component, OnInit, Input } from '@angular/core';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { Router } from '@angular/router';
import { CharacteristicValue } from '../characteristic-value';
import { CharacteristicValueService } from '../characteristic-value.service';

@Component({
  selector: 'app-characteristic-value-index',
  templateUrl: './characteristic-value-index.component.html',
  styleUrls: ['./characteristic-value-index.component.css']
})
export class CharacteristicValueIndexComponent implements OnInit {
  protected characteristicId: number = 0;
  protected characteristicValues: CharacteristicValue[] = [];
  @Input() administrating: boolean;


  constructor(
    private characteristicValueService: CharacteristicValueService,
    private parameterService: ParameterService,
    private router: Router) { }

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

  create() {
    let characteristicId = this.parameterService.getParam(ParameterNames.characteristicId);
    this.router.navigate(['/characteristicValues/create'], { queryParams: { characteristicId: characteristicId } });
  }
}
