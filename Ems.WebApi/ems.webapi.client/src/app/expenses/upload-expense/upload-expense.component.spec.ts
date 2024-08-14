import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadExpenseComponent } from './upload-expense.component';

describe('UploadExpenseComponent', () => {
  let component: UploadExpenseComponent;
  let fixture: ComponentFixture<UploadExpenseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UploadExpenseComponent]
    });
    fixture = TestBed.createComponent(UploadExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
