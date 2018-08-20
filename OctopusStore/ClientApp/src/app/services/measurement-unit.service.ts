import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataReadService } from './data-read-service';
import { MessageService } from './message.service';
import { MeasurementUnitDetails } from '../view-models/measurement-unit/measurement-unit-details';
import { MeasurementUnitIndex } from '../view-models/measurement-unit/measurement-unit-index';
import { MeasurementUnit } from '../view-models/measurement-unit/measurement-unit';

@Injectable({
  providedIn: 'root'
})
export class MeasurementUnitService extends DataReadService<
  MeasurementUnit,
  MeasurementUnitIndex,
  MeasurementUnitDetails> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/measurementUnits';
    this.serviceName = 'Measurement unit service';
  }
}
