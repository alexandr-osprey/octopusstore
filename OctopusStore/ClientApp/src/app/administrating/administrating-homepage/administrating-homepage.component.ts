import { Component, OnInit } from '@angular/core';
import { ParameterService } from 'src/app/services/parameter.service';
import { ParameterNames } from 'src/app/services/parameter-names';
import { Category } from 'src/app/view-models/category/category';
import { Router } from '@angular/router';

@Component({
  selector: 'app-administrating-homepage',
  templateUrl: './administrating-homepage.component.html',
  styleUrls: ['./administrating-homepage.component.css']
})
export class AdministratingHomepageComponent implements OnInit {
  constructor(private rouer: Router, private parameterService: ParameterService) { }

  ngOnInit() {
  }

  initializeComponent() {
    this.parameterService.params$.subscribe(params => {
      let updated: number = +this.parameterService.getParam(ParameterNames.categoryId);
    });
  }

  categorySaved(category: Category) {
    this.parameterService.navigateWithUpdatedParams([ParameterNames.categoryId, category.id]);
  }
}
