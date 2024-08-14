import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ExpenseService } from 'src/services/expense.service';
import { Expense, ExpenseRequestModel } from 'src/app/models/expense.model';

@Component({
  selector: 'app-expense-modal',
  templateUrl: './expense-modal.component.html'
})
export class ExpenseModalComponent implements OnInit {
  @Input() expense!: Expense;
  expenseForm!: FormGroup;
 expenseModel: ExpenseRequestModel = new ExpenseRequestModel();
  constructor(
    public activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private expenseService: ExpenseService
  ) { }

  ngOnInit(): void {
    this.expenseForm = this.fb.group({
      transactionDate: [this.formatDate(this.expense.transactionDate.toString()), Validators.required],
      transactionNumber: [this.expense.transactionNumber, Validators.required],
      transactionDetails: [this.expense.transactionDetails, Validators.required],
      transactionAmount: [this.expense.transactionAmount, Validators.required],
      balance: [this.expense.balance, Validators.required],
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }

  onSubmit(): void {debugger
    if (this.expenseForm.valid) {
      this.expenseModel.model = { ...this.expense, ...this.expenseForm.value };
      this.expenseService.updateExpense(this.expenseModel,this.expense.id).subscribe(() => {
        this.activeModal.close('saved');
      });
    }
  }
  cancel(): void {debugger
    this.activeModal.dismiss('cancel');
  }
}
