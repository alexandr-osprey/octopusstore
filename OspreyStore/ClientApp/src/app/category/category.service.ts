import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { APP_SETTINGS } from '../app-settings';
import { DataReadWriteService } from '../services/data-read-write.service';
import { Category } from './category';
import { IdentityService } from '../identity/identity.service';
import { MessageService } from '../message/message.service';

@Injectable({
  providedIn: 'root'
})
export class CategoryService extends DataReadWriteService<Category> {
  public rootCategory: Category = new Category(APP_SETTINGS.rootCategory);

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/categories';
    this.serviceName = 'Category service';
  }

}
