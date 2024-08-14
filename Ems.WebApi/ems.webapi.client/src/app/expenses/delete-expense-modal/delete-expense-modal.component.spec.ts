import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteExpenseModalComponent } from './delete-expense-modal.component';

describe('DeleteExpenseModalComponent', () => {
  let component: DeleteExpenseModalComponent;
  let fixture: ComponentFixture<DeleteExpenseModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DeleteExpenseModalComponent]
    });
    fixture = TestBed.createComponent(DeleteExpenseModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
