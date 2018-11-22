import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';
import { CharacteristicValueService } from '../../services/characteristic-value.service';
import { ParameterService } from 'src/app/services/parameter.service';

@Component({
  selector: 'app-characteristic-pane',
  templateUrl: './characteristic-pane.component.html',
  styleUrls: ['./characteristic-pane.component.css']
})
export class CharacteristicPaneComponent implements OnInit {
  @Input() characteristic: Characteristic;
  @Output() characteristicValueSelected = new EventEmitter<CharacteristicValue>();
  @Output() characteristicValueUnselected = new EventEmitter<CharacteristicValue>();
  public pickedValue
  public characteristicValues: CharacteristicValue[] = [];

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
    this.characteristicValues = [];
    if (this.characteristic) {
      this.characteristicValueService.index({ characteristicId: this.characteristic.id })
        .subscribe(data => {
          data.entities.forEach(e => {
            this.characteristicValues.push(new CharacteristicValue(e));
          });
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
