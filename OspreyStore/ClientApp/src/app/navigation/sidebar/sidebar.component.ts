import { Component, OnInit } from '@angular/core';
import { ParameterService } from 'src/app/parameter/parameter.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  storeView: boolean;
  constructor(private parameterService: ParameterService) { }

  ngOnInit() {
    if (this.parameterService.getCurrentUrlWithoutParams().toLowerCase().indexOf('stores') >= 0) {
      this.storeView = true;
    }
  }

}
