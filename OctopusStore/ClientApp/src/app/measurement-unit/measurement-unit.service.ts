import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { MeasurementUnit } from './measurement-unit';
import { DataReadWriteService } from '../services/data-read-write.service';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';


@Injectable({
  providedIn: 'root'
})
export class MeasurementUnitService extends DataReadWriteService<MeasurementUnit> {

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
