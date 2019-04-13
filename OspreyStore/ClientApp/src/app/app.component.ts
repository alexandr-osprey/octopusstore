import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import {  slideInAnimation } from './route-animations';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  changeDetection: ChangeDetectionStrategy.Default,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [
   // slider,
     slideInAnimation,
  ],
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

  prepareRoute(outlet: RouterOutlet) {
    return outlet && outlet.activatedRouteData && outlet.activatedRouteData['animation'];
  }
  //public constructor() {
  //  document.getElementsByTagName("html")[0].style.height = "100%";
  //  document.body.style.height = "100%";
  //  (document.getElementsByTagName("app-root")[0] as HTMLHtmlElement).style.height = "100%";
  //}
}
