import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { DataReadService } from './data-read-service';
import { CategoryIndex } from '../view-models/category/category-index';
import { CategoryDetail } from '../view-models/category/category-detail';
import { Category } from '../view-models/category/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService extends DataReadService<Category, CategoryIndex, CategoryDetail> {
  public static rootCategoryId: number = 1;

  constructor(
    protected http: HttpClient,
    protected messageService: MessageService) {
    super(http, messageService);
    this.remoteUrl = 'api/categories';
    this.serviceName = 'Category service';
  }
}
