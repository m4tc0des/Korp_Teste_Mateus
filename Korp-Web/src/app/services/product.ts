import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Produto } from '../models/produto';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = 'https://localhost:7265/api/products'; 
  
  private _refreshNeeded$ = new Subject<void>();

  get refreshNeeded$() {
    return this._refreshNeeded$;
  }

  constructor(private http: HttpClient) {}

  getProducts(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl);
  }

  cadastrarProduto(produto: any): Observable<any> {
    return this.http.post(this.apiUrl, produto);
  }

  baixarEstoque(id: number, quantidade: number): Observable<any> {
  return this.http.post(`${this.apiUrl}/${id}/baixar-estoque`, { quantidade });
}
}