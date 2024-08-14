import { Component } from '@angular/core';
import { ExpenseService } from 'src/services/expense.service';

@Component({
  selector: 'app-upload-expense',
  templateUrl: './upload-expense.component.html',
  styleUrls: ['./upload-expense.component.css']
})
export class UploadExpenseComponent {

  selectedFile: File | null = null;

  constructor(private expenseService: ExpenseService) { }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onUpload(): void {
    if (this.selectedFile) {
      this.expenseService.uploadFile(this.selectedFile).subscribe(
        (response: any) => {
          console.log('File uploaded successfully', response);
        },
        (error: any) => {
          console.error('File upload failed', error);
        }
      );
    }
  }
}
