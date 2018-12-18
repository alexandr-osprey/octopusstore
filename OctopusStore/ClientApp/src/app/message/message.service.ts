import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Message, MessageType } from '../message/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  protected messageSource = new Subject<Message>();
  message$ = this.messageSource.asObservable();
  messages: Message[] = [];

  public sendMessage(content: string, type: MessageType = MessageType.Info) {
    let message = new Message(content, type);
    this.messages.push(message);
    this.messageSource.next(message);
  }
  public sendTrace(content: string) {
    console.log(content);
    //this.sendMessage(content, MessageType.Trace);
  }
  public sendInfo(content: string) {
    this.sendMessage(content, MessageType.Info);
  }
  public sendSuccess(content: string) {
    this.sendMessage(content, MessageType.Success);
  }
  public sendWarn(content: string) {
    this.sendMessage(content, MessageType.Warn);
  }
  public sendError(content: string) {
    this.sendMessage(content, MessageType.Error);
  }

  clear() {
    this.messages = [];
  }
  constructor() { }
}

