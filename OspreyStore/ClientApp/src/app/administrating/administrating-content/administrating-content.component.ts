import { Component, OnInit } from '@angular/core';
import { ParameterService } from 'src/app/parameter/parameter.service';
import { ParameterNames } from 'src/app/parameter/parameter-names';
import { Category } from 'src/app/category/category';
import { MessageService } from '../../message/message.service';

@Component({
  selector: 'app-administrating-content',
  templateUrl: './administrating-content.component.html',
  styleUrls: ['./administrating-content.component.css']
})
export class AdministratingContentComponent implements OnInit {
  constructor(
    private messageService: MessageService,
    private parameterService: ParameterService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.parameterService.params$.subscribe(params => {
      let updated: number = +this.parameterService.getParam(ParameterNames.categoryId);
    });

    //localStorage.removeItem('administrating-content-help-shown');
    if (!localStorage.getItem('administrating-content-help-shown')) {
      this.showHelpMessages();
      localStorage.setItem('administrating-content-help-shown', 'true');
    }
  }

  showHelpMessages() {
    this.messageService.delay(1 * 1000).then(() =>
      this.messageService.sendHelp("Website administrator are able to create and update and delete brands, categories, characteristics and characteristic values"));
  }

  categorySaved(category: Category) {
    this.parameterService.navigateWithUpdatedParams({ "categoryId": category.id, });
  }
}
