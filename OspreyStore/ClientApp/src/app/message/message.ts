export class Message {
  content: string;
  style: string;
  messageType: MessageType;
  dismissed: boolean = false;

  public constructor(content: string, type: MessageType) {
    this.content = content;
    this.style = MessageType[type];
    this.messageType = type;
    this.dismissed = false;
  }
}
export enum MessageType {
  Trace,
  Info,
  Success,
  Warn,
  Error,
  Help
}
