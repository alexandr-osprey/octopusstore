import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MeasurementUnit } from 'src/app/view-models/measurement-unit/measurement-unit';
import { MeasurementUnitService } from 'src/app/services/measurement-unit.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-measurement-unit-index',
  templateUrl: './measurement-unit-index.component.html',
  styleUrls: ['./measurement-unit-index.component.css']
})
export class MeasurementUnitIndexComponent implements OnInit {
  measurementUnits: MeasurementUnit[] = [];
  @Input() administrating: boolean;
  

  constructor(
    private measurementUnitService: MeasurementUnitService,
    private router: Router) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.measurementUnitService.index().subscribe(data => {
      if (data) {
        this.measurementUnits = [];
        data.entities.forEach(m => {
          this.measurementUnits.push(new MeasurementUnit(m));
        });
      }
    });
  }

  create() {
    this.router.navigate(['/measurementUnits/create']);
  }

}
