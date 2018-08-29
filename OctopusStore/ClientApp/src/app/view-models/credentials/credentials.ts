export class Credentials {
  email: string;
  password: string;

  public constructor(init?: Partial<Credentials>) {
    Object.assign(this, init);
  }
}
