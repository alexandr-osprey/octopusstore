import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryCreateUpdateComponent } from './category-create-update.component';

describe('CategoryCreateUpdateComponent', () => {
  let component: CategoryCreateUpdateComponent;
  let fixture: ComponentFixture<CategoryCreateUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoryCreateUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryCreateUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
