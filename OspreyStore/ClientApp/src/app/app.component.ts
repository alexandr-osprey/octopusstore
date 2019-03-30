import { Component, OnInit } from '@angular/core';
import { ParameterService } from './parameter/parameter.service';
import { ParameterNames } from './parameter/parameter-names';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [],
})
export class AppComponent implements OnInit {
  title = 'app';
  areActionsActive: boolean = false;

  public constructor(private parameterService: ParameterService) { }


  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.parameterService.params$.subscribe(_ => {
      if (this.parameterService.isParamChanged(ParameterNames.actionsShown)) {
        this.areActionsActive = this.parameterService.getParam(ParameterNames.actionsShown) as boolean;
      }
    });
  }
  //public constructor() {
  //  document.getElementsByTagName("html")[0].style.height = "100%";
  //  document.body.style.height = "100%";
  //  (document.getElementsByTagName("app-root")[0] as HTMLHtmlElement).style.height = "100%";
  //}
}
