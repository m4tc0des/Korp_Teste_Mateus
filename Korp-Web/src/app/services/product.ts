import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Produto } from '../models/produto';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = 'https://localhost:7265/api/products'; // Sua URL da API
  
  // O "pulo do gato" do RxJS para atualizar a lista
  private _refreshNeeded$ = new Subject<void>();

  get refreshNeeded$() {
    return this._refreshNeeded$;
  }

  constructor(private http: HttpClient) {}

  getProducts(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl);
  }

  // Novo método de cadastro
  cadastrarProduto(produto: any): Observable<any> {
    return this.http.post(this.apiUrl, produto);
  }

  baixarEstoque(codigo: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${codigo}/baixar-estoque?quantidade=1`, {});
  }
}