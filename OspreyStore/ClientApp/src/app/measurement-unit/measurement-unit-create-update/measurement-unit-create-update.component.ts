import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { MeasurementUnit } from '../measurement-unit';
import { MeasurementUnitService } from '../measurement-unit.service';
import { MessageService } from 'src/app/message/message.service';

@Component({
  selector: 'app-measurement-unit-create-update',
  templateUrl: './measurement-unit-create-update.component.html',
  styleUrls: ['./measurement-unit-create-update.component.css']
})
export class MeasurementUnitCreateUpdateComponent implements OnInit {
  measurementUnit: MeasurementUnit;
  isUpdating: boolean = false;
  @Output() measurementUnitSaved = new EventEmitter<MeasurementUnit>();

  constructor(
    private measurementUnitService: MeasurementUnitService,
    private route: ActivatedRoute,
    private messageService: MessageService,
    private location: Location) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    let id = +this.route.snapshot.paramMap.get('id') || 0;
    if (id != 0) {
      this.isUpdating = true;
      this.measurementUnitService.get(id).subscribe(data => {
        if (data) {
          this.measurementUnit = new MeasurementUnit(data);
        }
      });
    } else {
      this.measurementUnit = new MeasurementUnit();
      this.isUpdating = false;
    }
  }

  createOrUpdate() {
    this.measurementUnitService.postOrPut(this.measurementUnit).subscribe(
      (data) => {
        if (data) {
          this.measurementUnit = new MeasurementUnit(data);
          this.messageService.sendSuccess("Measurement unit saved");
          this.measurementUnitSaved.emit(this.measurementUnit);
          if (this.measurementUnitSaved.observers.length == 0) {
            this.location.back();
          }
        }
      });
  }

}
