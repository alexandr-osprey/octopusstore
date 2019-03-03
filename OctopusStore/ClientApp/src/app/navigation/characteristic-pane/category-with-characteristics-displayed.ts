import { Category } from "src/app/category/category";
import { Characteristic } from "src/app/characteristic/characteristic";

export class CategoryWithCharacteristicsDisplayed extends Category {
  characteristics: Characteristic[];

  public constructor(init?: Partial<CategoryWithCharacteristicsDisplayed>) {
    super(init);
    Object.assign(this, init);
  }
}
