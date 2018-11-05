export class TokenPair {
  public token: string;
  public refreshToken: string;

  public constructor(init?: Partial<TokenPair>) {
    Object.assign(this, init);
  }
}
