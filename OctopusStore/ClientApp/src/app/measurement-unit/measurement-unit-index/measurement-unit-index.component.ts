import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MeasurementUnit } from '../measurement-unit';
import { MeasurementUnitService } from '../measurement-unit.service';

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
