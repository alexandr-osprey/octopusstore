import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataReadService } from './data-read-service';
import { MessageService } from './message.service';
import { CharacteristicIndex } from '../view-models/characteristic/characteristic-index';
import { CharacteristicDetail } from '../view-models/characteristic/characteristic-detail';
import { Characteristic } from '../view-models/characteristic/characteristic';

@Injectable({
  providedIn: 'root'
})
export class CharacteristicService extends DataReadService<Characteristic, CharacteristicIndex, CharacteristicDetail> {

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/characteristics';
    this.serviceName = 'Characteristic service';
  }
}
