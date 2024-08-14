import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Expense } from 'src/app/models/expense.model';

@Component({
  selector: 'app-delete-expense-modal',
  templateUrl: './delete-expense-modal.component.html',
  styleUrls: ['./delete-expense-modal.component.css']
})
export class DeleteExpenseModalComponent {
  @Input() expense!: Expense;

  constructor(public activeModal: NgbActiveModal) {}

  confirmDelete(): void {
    this.activeModal.close('delete');
  }

  cancel(): void {
    this.activeModal.dismiss('cancel');
  }
}
