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

  public constructor() { }


  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
  }
  //public constructor() {
  //  document.getElementsByTagName("html")[0].style.height = "100%";
  //  document.body.style.height = "100%";
  //  (document.getElementsByTagName("app-root")[0] as HTMLHtmlElement).style.height = "100%";
  //}
}
