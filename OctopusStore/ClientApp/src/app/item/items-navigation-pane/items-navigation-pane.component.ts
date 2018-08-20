import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-items-navigation-pane',
  templateUrl: './items-navigation-pane.component.html',
  styleUrls: ['./items-navigation-pane.component.css']
})
export class ItemsNavigationPaneComponent implements OnInit {

  //@Output() paramsUpdatedEvent = new EventEmitter<any>();
  @Input() storeId: number;

  constructor() { }

  ngOnInit() {
  }

  paramsUpdated(updatedParams: any) {
    //this.paramsUpdatedEvent.emit(updatedParams);
  }
}
