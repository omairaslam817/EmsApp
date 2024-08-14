import { Component, OnInit, ViewChild } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Expense } from 'src/app/models/expense.model';
import { ExpenseService } from 'src/services/expense.service';
import { ExpenseModalComponent } from '../expense-modal/expense-modal.component';
import { DeleteExpenseModalComponent } from '../delete-expense-modal/delete-expense-modal.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort, Sort } from '@angular/material/sort';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css']
})
export class ExpenseListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'transactionDate', 'transactionNumber', 'transactionDetails', 'transactionAmount', 'balance', 'actions'];
  dataSource = new MatTableDataSource<Expense>([]);

  allFactories: Expense[] = [];
  totalRows = 0;
  pageSize = 5;
  currentPage = 0;
  defaultPageSize = 10;
  pageSizeOptions: number[] = [3, 10, 25, 100];
  searchValue: string = "";
  loadingIndicator!: boolean;
  sortColumn: string = 'id';
  sortOrder: string = 'asc';
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private expenseService: ExpenseService,
    private router: Router,
    private modalService: NgbModal,
    private snackBar: MatSnackBar

  ) { }

  ngOnInit(): void {

    this.loadData();
  }
  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe((sort: Sort) => {
      this.sortColumn = sort.active;
      this.sortOrder = sort.direction;
      this.loadData();
    });
  }



  loadData(): void {
    this.loadingIndicator = true;
    this.expenseService.getExpenses(this.currentPage + 1, this.pageSize, this.searchValue, this.sortColumn, this.sortOrder).subscribe((data: any) => {
      this.dataSource = data.items;
      this.loadingIndicator = false;
      this.dataSource.data = data.items;
      this.totalRows = data.totalCount;
    });
  }

  editFactory(id: number) {
    this.router.navigate(['/add-edit-factory'], { queryParams: { id: id } });
  }
  onSearchChanged(value: string) {
    this.searchValue = value;
    this.currentPage = 0;
    this.loadData();
  }
  pageChanged(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    this.loadData();
  }

  deleteExpense(expense: Expense): void {
    const modalRef = this.modalService.open(DeleteExpenseModalComponent);
    modalRef.componentInstance.expense = expense;

    modalRef.result.then(
      (result) => {
        if (result === 'delete') {
          this.expenseService.deleteExpense(expense.id).subscribe(() => {
            this.snackBar.open('Expense deleted successfully', 'Close', {
              duration: 2000,
            });
            this.loadData();
          }, error => {
            this.snackBar.open('Error deleting expense', 'Close', {
              duration: 2000,
            });
          });
        }
      },
      (reason) => {
        console.log('Modal dismissed:', reason);
      }
    );
  }

  editExpense(expense: Expense): void {
    const modalRef = this.modalService.open(ExpenseModalComponent, { size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.expense = expense;

    modalRef.result.then((result) => {
      if (result === 'saved') {
        this.snackBar.open('Expense updated successfully', 'Close', {
          duration: 2000,
        });
        this.loadData();
      }
    }, (reason) => {
      console.log('Modal dismissed:', reason);
    }).catch((error) => {
      this.snackBar.open('Error updating expense', 'Close', {
        duration: 2000,
      });
      console.error(error);
    },
    );
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.loadingIndicator = true;
      this.expenseService.uploadFile(file).subscribe(
        response => {
          this.snackBar.open('File uploaded successfully', 'Close', {
            duration: 2000,
          });
          this.loadData();
          this.loadingIndicator = false;

        },
        error => {
          this.snackBar.open('Error while expense uploading', 'Close', {
            duration: 2000,
          });
          this.loadingIndicator = false;

        }
      );
    }
  }
}
