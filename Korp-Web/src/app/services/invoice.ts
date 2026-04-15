import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private apiUrl = 'https://localhost:7232/api/invoices';

  constructor(private http: HttpClient) {}

  getInvoices(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}