import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { CharacteristicValue } from 'src/app/characteristic-value/characteristic-value';
import { CharacteristicValueService } from 'src/app/characteristic-value/characteristic-value.service';
import { Characteristic } from 'src/app/characteristic/characteristic';
import { CharacteristicValueDisplayed } from './characteristic-value-displayed';

@Component({
  selector: 'app-characteristic-pane',
  templateUrl: './characteristic-pane.component.html',
  styleUrls: ['./characteristic-pane.component.css']
})
export class CharacteristicPaneComponent implements OnInit {
  @Input() characteristic: Characteristic;
  @Output() characteristicValueSelected = new EventEmitter<CharacteristicValue>();
  @Output() characteristicValueUnselected = new EventEmitter<CharacteristicValue>();
  public characteristicValuesDisplayed: CharacteristicValueDisplayed[] = [];

  constructor(
    private characteristicValueService: CharacteristicValueService,
    private parameterService: ParameterService) {

  }

  ngOnInit() {
    //this.initializeComponent();
  }

  ngOnChanges() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.characteristicValuesDisplayed = [];
    if (this.characteristic) {
      this.characteristicValueService.index({ characteristicId: this.characteristic.id })
        .subscribe(data => {
          data.entities.forEach(e => {
            this.characteristicValuesDisplayed.push(new CharacteristicValueDisplayed(e));
          });
          let filters: string = this.parameterService.getParam(ParameterNames.characteristicsFilter);
          if (filters) {
            let values = filters.split(';').map(v => +v);
            let valuesToSelect = this.characteristicValuesDisplayed.filter(v => values.includes(v.id));
            valuesToSelect.forEach(v => this.characteristicValuesDisplayed.find(c => c == v).selected = true);
          }
        });
    }
  }

  characteristicValueClick(characteristicValue: CharacteristicValueDisplayed) {
    characteristicValue.selected = !characteristicValue.selected;
    if (characteristicValue.selected) {
      this.characteristicValueSelected.emit(characteristicValue);
    } else {
      this.characteristicValueUnselected.emit(characteristicValue);
    }
  }

  characteristicValueChecked(evt, characteristicValue) {
    var target = evt.target;
    if (target.checked) {
      
     // doSelected(target);
     // this._prevSelected = target;
    } else {
      this.characteristicValueUnselected.emit(characteristicValue);
    //  doUnSelected(this._prevSelected)
    }
  }
}
