import { Injectable } from '@angular/core';
import { Category } from '../view-models/category/category';
import { HttpClient } from '@angular/common/http';
import { APP_SETTINGS } from '../app-settings';

@Injectable()
export class AppLoadService {

  constructor(private httpClient: HttpClient) { }

  public getRootCategory(): Promise<any> {
    console.log("Before loading root category");
    const promise = this.httpClient.get<Category>('/api/categories/root').toPromise().then(data => {
      console.log("from api: " + data);
      APP_SETTINGS.rootCategoryId = data.id;
      return data;
    });
    return promise;
  }
}
