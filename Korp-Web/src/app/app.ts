import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ProductService } from './services/product';
import { Produto } from './models/produto';


@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.scss',
  standalone: false
})
export class AppComponent implements OnInit {
  public products: Produto[] = [];
  public loading: boolean = true;
  
  public showToast: boolean = false;
  public lastProductUpdated: string = '';

  public novoProduto = { codigo: '', descricao: '', saldo: 0 };

  constructor(private productService: ProductService, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
  this.listProducts();

  this.productService.refreshNeeded$.subscribe(() => {
    console.log('Atualizando lista via RxJS...');
    this.listProducts();
  });
}

  listProducts() {
  this.productService.getProducts().subscribe({
    next: (data) => {
      this.products = [...data]; 
      this.loading = false;
      this.cd.detectChanges(); 
    },
    error: (err) => {
      console.error(err);
      this.loading = false;
    }
  });
}

 salvarProduto() {
  if (!this.novoProduto.codigo || !this.novoProduto.descricao) {
    alert('Preencha os campos obrigatórios!');
    return;
  }

  this.productService.cadastrarProduto(this.novoProduto).subscribe({
    next: (res) => {
      console.log('Sucesso ao cadastrar:', res);
      this.showToast = true;
      this.lastProductUpdated = 'Produto cadastrado com sucesso!';
      
      this.novoProduto = { codigo: '', descricao: '', saldo: 0 };
      
      this.productService.refreshNeeded$.next();
      
      setTimeout(() => {
        this.showToast = false;
        this.cd.detectChanges();
      }, 3000);
    },
    error: (err) => {
      console.error('Erro detalhado da API:', err);

      const mensagemErro = err.error?.message || err.error || 'Erro desconhecido ao cadastrar.';
      
      alert('Falha no cadastro: ' + mensagemErro);
    }
  });
}

baixar(produto: Produto) {
  this.productService.baixarEstoque(produto.id, 1).subscribe({
    next: () => {
      this.showToast = true;
      this.listProducts(); 

      setTimeout(() => {
        this.showToast = false;
        this.cd.detectChanges();
      }, 3000);
    },
    error: (err) => console.error("Erro ao baixar estoque", err)
  });
}
}