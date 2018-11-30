import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { Category } from '../view-models/category/category';
import { IdentityService } from './identity.service';
import { DataReadWriteService } from './data-read-write.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { APP_SETTINGS } from '../app-settings';

@Injectable({
  providedIn: 'root'
})
export class CategoryService extends DataReadWriteService<Category> {
  public rootCategoryId: number = APP_SETTINGS.rootCategoryId;

  constructor(
    protected http: HttpClient,
    protected router: Router,
    protected identityService: IdentityService,
    protected messageService: MessageService) {
    super(http, router, identityService, messageService);
    this.remoteUrl = '/api/categories';
    this.serviceName = 'Category service';

    this.getRoot().subscribe(c => {
      if (c)
        this.rootCategoryId = c.id;
    });
  }

  public getRoot(): Observable<Category> {
    return this.getCustom(this.remoteUrl + '/root', {}, this.defaultHttpHeaders);
  }
}
