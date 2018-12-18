import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { MessageService } from './message.service';
import { Subscription } from 'rxjs';
import { Message } from './message'

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  protected messageSubscription: Subscription;
  protected messages: Message[] = [];
  protected messageTimeout = 3 * 1000;

  constructor(protected messageService: MessageService) { }

  ngOnInit() {
    this.initializeComponent();
  }

  initializeComponent() {
    this.messageSubscription = this.messageService.message$.subscribe(message => {
      console.log('message ' + message.content);
      console.log('befor push message array length: ' + this.messages.length);
      this.delay(100).then(() => this.messages.push(message));
      console.log('after push message array length: ' + this.messages.length);
      this.delay(this.messageTimeout).then(() => {
        //this.messages.push(message);
        this.dismiss(message)
        console.log('after dismiss message array length: ' + this.messages.length);
      });
    });
  }

  dismiss(message: Message) {
    this.messages = this.messages.filter(m => m != message);
  }
  protected delay(ms: number): Promise<{}> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
