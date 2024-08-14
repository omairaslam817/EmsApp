export interface Expense {
    id: string;
    transactionDate: Date;
    transactionNumber: string;
    transactionDetails: string;
    transactionAmount: number;
    balance: number;
  }
  export class ExpenseRequestModel {
   model!:{
    id: string;
    transactionDate: Date;
    transactionNumber: string;
    transactionDetails: string;
    transactionAmount: number;
    balance: number;
   } 
  }
  