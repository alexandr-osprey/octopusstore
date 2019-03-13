import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministratingStoreComponent } from './administrating-store.component';

describe('AdministratingStoreComponent', () => {
  let component: AdministratingStoreComponent;
  let fixture: ComponentFixture<AdministratingStoreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministratingStoreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministratingStoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
