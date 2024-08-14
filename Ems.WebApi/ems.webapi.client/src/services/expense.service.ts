import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Expense, ExpenseRequestModel } from 'src/app/models/expense.model';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {


  constructor(private http: HttpClient) { }

  private apiUrl = 'https://localhost:7112'; // Base URL
uploadFile(file: File): Observable<any> {
  const formData = new FormData();
  formData.append('file', file);
  return this.http.post(`${this.apiUrl}/Expense/upload`, formData);
}

  

  getExpenses(page?: number, pageSize?: number, searchTerm?: string, sortColumn?: string, sortOrder?: string): Observable<Expense[]> {
    const endpointUrl = page && pageSize ? `${this.apiUrl}/Expense/AllExpenses?pageNumber=${page}&pageSize=${pageSize}&searchTerm=${searchTerm}&sortColumn=${sortColumn}&sortOrder=${sortOrder}` : this.apiUrl;
    return this.http.get<Expense[]>(endpointUrl);
  }

  getExpenseById(id: number): Observable<Expense> {
    return this.http.get<Expense>(`${this.apiUrl}/expenses/${id}`);
  }

  createExpense(expense: Expense): Observable<Expense> {
    return this.http.post<Expense>(`${this.apiUrl}/Expense/expenses`, expense);
  }

  updateExpense(expense: ExpenseRequestModel,id:string): Observable<Expense> {
    return this.http.put<Expense>(`${this.apiUrl}/Expense/${id}`, expense);
  }

  deleteExpense(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Expense/${id}`);
  }
}

