import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';
import { CharacteristicValueService } from '../../services/characteristic-value.service';
import { ParameterService } from 'src/app/services/parameter.service';
import { ParameterNames } from 'src/app/services/parameter-names';
import { CharacteristicValueCheckbox } from 'src/app/view-models/characteristic-value/characteristic-value-checkbox';

@Component({
  selector: 'app-characteristic-pane',
  templateUrl: './characteristic-pane.component.html',
  styleUrls: ['./characteristic-pane.component.css']
})
export class CharacteristicPaneComponent implements OnInit {
  @Input() characteristic: Characteristic;
  @Output() characteristicValueSelected = new EventEmitter<CharacteristicValue>();
  @Output() characteristicValueUnselected = new EventEmitter<CharacteristicValue>();
  public characteristicValueCheckboxes: CharacteristicValueCheckbox[] = [];

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
    this.characteristicValueCheckboxes = [];
    if (this.characteristic) {
      this.characteristicValueService.index({ characteristicId: this.characteristic.id })
        .subscribe(data => {
          data.entities.forEach(e => {
            this.characteristicValueCheckboxes.push(new CharacteristicValueCheckbox(e));
          });
          let filters: string = this.parameterService.getParam(ParameterNames.characteristicsFilter);
          let values = filters.split(';').map(v => +v);
          let valuesToCheck = this.characteristicValueCheckboxes.filter(v => values.includes(v.id));
          valuesToCheck.forEach(v => this.characteristicValueCheckboxes.find(c => c == v).checked = true);
        });
    }
  }

  characteristicValueChecked(evt, characteristicValue) {
    var target = evt.target;
    if (target.checked) {
      this.characteristicValueSelected.emit(characteristicValue);
     // doSelected(target);
     // this._prevSelected = target;
    } else {
      this.characteristicValueUnselected.emit(characteristicValue);
    //  doUnSelected(this._prevSelected)
    }
  }
}
