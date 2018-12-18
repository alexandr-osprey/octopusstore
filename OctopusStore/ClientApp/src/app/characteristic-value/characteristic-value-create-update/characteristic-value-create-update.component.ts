import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { CharacteristicValue } from '../characteristic-value';
import { Characteristic } from 'src/app/characteristic/characteristic';
import { CharacteristicService } from 'src/app/characteristic/characteristic.service';
import { MessageService } from 'src/app/message/message.service';
import { CharacteristicValueService } from '../characteristic-value.service';

@Component({
  selector: 'app-characteristic-value-create-update',
  templateUrl: './characteristic-value-create-update.component.html',
  styleUrls: ['./characteristic-value-create-update.component.css']
})
export class CharacteristicValueCreateUpdateComponent implements OnInit {
  protected characteristicValue: CharacteristicValue;
  protected characteristics: Characteristic[] = [];
  public isUpdating = false;
  @Output() characteristicValueSaved = new EventEmitter<CharacteristicValue>();

  constructor(
    private characteristicService: CharacteristicService,
    private route: ActivatedRoute,
    private messageService: MessageService,
    private characteristicValueService: CharacteristicValueService,
    private parameterService: ParameterService,
    private location: Location) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.updateCharacteristics();
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id != 0) {
      this.isUpdating = true;
      this.characteristicValueService.get(id).subscribe(data => {
        if (data) {
          this.characteristicValue = new CharacteristicValue(data);
        }
      });
    } else {
      let characteristicId = +this.parameterService.getParam(ParameterNames.characteristicId);
      this.characteristicValue = new CharacteristicValue({ characteristicId: characteristicId });
      this.isUpdating = false;
    }
  }

  updateCharacteristics() {
    this.characteristicService.index().subscribe(data => {
      if (data) {
        this.characteristics = [];
        data.entities.forEach(c => {
          this.characteristics.push(new Characteristic(c));
        });
      }
    });
  }

  createOrUpdate() {
    this.characteristicValueService.postOrPut(this.characteristicValue).subscribe(
      (data) => {
        if (data) {
          this.characteristicValue = new CharacteristicValue(data);
          this.messageService.sendSuccess("Characteristic value saved");
          this.characteristicValueSaved.emit(this.characteristicValue);
          if (this.characteristicValueSaved.observers.length == 0) {
            this.location.back();
          }
        }
      });
  }

}
