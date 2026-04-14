import { Component, OnInit } from '@angular/core';
import { ProductService } from './services/product';
import { Produto } from './models/produto';       

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.scss'
})
export class AppComponent implements OnInit {
  // Criamos uma lista vazia para segurar os produtos
  public products: Produto[] = [];

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.listProducts();
  }

  listProducts() {
    this.productService.getProducts().subscribe({
      next: (data) => {
        this.products = data;
        console.log('Produtos carregados:', data);
      },
      error: (err) => {
        console.error('Erro ao buscar produtos. Verifique se a API está rodando e o CORS liberado!', err);
      }
    });
  }
}