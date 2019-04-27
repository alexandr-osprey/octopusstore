import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { MessageService } from './message.service';
import { Subscription } from 'rxjs';
import { Message, MessageType } from './message'

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  protected messages: Message[] = [];
  protected messageTimeout = 5 * 1000;
  //protected messageHelpTimeout = this.messageTimeout * 2;

  constructor(protected messageService: MessageService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.messageService.message$.subscribe(message => {
      this.delay(100).then(() => this.messages.push(message));
      let timeout = this.messageTimeout;
      if (message.messageType <= MessageType.Success) {
        this.delay(timeout).then(() => {
          this.dismiss(message)
        });
      }
      
    });
  }

  dismiss(message: Message) {
    this.messages = this.messages.filter(m => m != message);
  }
  protected delay(ms: number): Promise<{}> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
