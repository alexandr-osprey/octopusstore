export class Response {
  message: string;

  public constructor(init?: Partial<Response>) {
    this.message = "";
    Object.assign(this, init);
  }

  public toString() {
    return `${this.message}`;
  }
}
