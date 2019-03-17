export class Index<T> {
  page: number;
  totalPages: number;
  totalCount: number;
  entities: T[];

  public constructor(init?: Partial<Index<T>>) {
    Object.assign(this, init);
  }

  public toString() {
    return `${this.constructor.name}: page: ${this.page}, totalPages: ${this.totalPages}, count: ${this.entities.length}`;
  }
}
