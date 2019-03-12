import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { Category } from 'src/app/category/category';

@Component({
  selector: 'app-administrating-content',
  templateUrl: './administrating-content.component.html',
  styleUrls: ['./administrating-content.component.css']
})
export class AdministratingContentComponent implements OnInit {
  constructor(private rouer: Router, private parameterService: ParameterService) { }

  ngOnInit() {
  }

  initializeComponent() {
    this.parameterService.params$.subscribe(params => {
      let updated: number = +this.parameterService.getParam(ParameterNames.categoryId);
    });
  }

  categorySaved(category: Category) {
    this.parameterService.navigateWithUpdatedParams({ "categoryId": category.id, });
  }
}
