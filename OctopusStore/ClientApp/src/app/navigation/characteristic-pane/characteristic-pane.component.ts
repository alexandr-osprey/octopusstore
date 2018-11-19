import { Component, OnInit, Input } from '@angular/core';
import { Characteristic } from '../../view-models/characteristic/characteristic';
import { CharacteristicValue } from '../../view-models/characteristic-value/characteristic-value';
import { CharacteristicValueService } from '../../services/characteristic-value.service';

@Component({
  selector: 'app-characteristic-pane',
  templateUrl: './characteristic-pane.component.html',
  styleUrls: ['./characteristic-pane.component.css']
})
export class CharacteristicPaneComponent implements OnInit {
  @Input() characteristic: Characteristic;
  public characteristicValues: CharacteristicValue[] = [];

  constructor(private characteristicValueService: CharacteristicValueService) {

  }

  ngOnInit() {
    this.initializeComponent();
  }

  ngOnChanges() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.characteristicValues = [];
    if (this.characteristic) {
      this.characteristicValueService.index({ characteristicId: this.characteristic.id })
        .subscribe(data => data.entities.forEach(e => this.characteristicValues.push(new CharacteristicValue(e)));
        );
    }
  }
}
