export class TokenPair {
  public token: string;
  public refreshToken: string;
  public invalid: boolean = false;

  public constructor(init?: Partial<TokenPair>) {
    Object.assign(this, init);
  }
}
