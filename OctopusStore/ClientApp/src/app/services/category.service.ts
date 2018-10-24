import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { CategoryIndex } from '../view-models/category/category-index';
import { CategoryDetails } from '../view-models/category/category-details';
import { Category } from '../view-models/category/category';
import { IdentityService } from './identity-service';
import { DataReadWriteService } from './data-read-write-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CategoryService extends DataReadWriteService<Category, CategoryIndex, CategoryDetails> {
  public static rootCategoryId: number = 1;

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
