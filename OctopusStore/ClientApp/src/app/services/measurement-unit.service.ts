import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { MeasurementUnitDetail } from '../view-models/measurement-unit/measurement-unit-detail';
import { MeasurementUnitIndex } from '../view-models/measurement-unit/measurement-unit-index';
import { MeasurementUnit } from '../view-models/measurement-unit/measurement-unit';
import { IdentityService } from './identity-service';
import { DataReadWriteService } from './data-read-write-service';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class MeasurementUnitService extends DataReadWriteService<
MeasurementUnit,
MeasurementUnitIndex,
MeasurementUnitDetail> {

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/measurementUnits';
    this.serviceName = 'Measurement unit service';
  }
}
