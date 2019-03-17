import { Component } from '@angular/core';
import {
  trigger,
  state,
  style,
  animate,
  transition,
  // ...
} from '@angular/animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [],
})
export class AppComponent {
  title = 'app';

  //public constructor() {
  //  document.getElementsByTagName("html")[0].style.height = "100%";
  //  document.body.style.height = "100%";
  //  (document.getElementsByTagName("app-root")[0] as HTMLHtmlElement).style.height = "100%";
  //}
}
