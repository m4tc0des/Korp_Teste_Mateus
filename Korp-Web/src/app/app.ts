import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { ProductService } from './services/product';
import { InvoiceService } from './services/invoiceservice';
import { Produto } from './models/produto';
import { Invoice } from './models/invoice'; 

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.scss',
  standalone: false
})
export class AppComponent implements OnInit {
  public products: Produto[] = [];
  public invoices: Invoice[] = [];
  public loading: boolean = true;
  
  public showToast: boolean = false;
  public lastProductUpdated: string = '';

  showToastError = false;
  errorMessage = '';

  public novoProduto = { codigo: '', descricao: '', saldo: 0 };

  constructor(
    private productService: ProductService, 
    private invoiceService: InvoiceService, 
    private http: HttpClient, 
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.listProducts();
    this.listInvoices();

    this.productService.refreshNeeded$.subscribe(() => {
      this.listProducts();
    });

    this.invoiceService.refreshNeeded$.subscribe(() => {
      this.listInvoices();
    });
  }

  listProducts(): void {
    this.productService.getProducts().subscribe({
      next: (data: Produto[]) => { 
        this.products = [...data]; 
        this.loading = false;
        this.cd.detectChanges(); 
      },
      error: (err: any) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  listInvoices(): void {
    this.invoiceService.getInvoices().subscribe({
      next: (data: Invoice[]) => {
        this.invoices = data;
        this.cd.detectChanges();
      },
      error: (err: any) => console.error("Erro ao carregar financeiro", err)
    });
  }

  salvarProduto(): void {
    if (!this.novoProduto.codigo || !this.novoProduto.descricao) {
      alert('Preencha os campos obrigatórios!');
      return;
    }

    this.productService.cadastrarProduto(this.novoProduto).subscribe({
      next: (res: any) => {
        this.showToast = true;
        this.lastProductUpdated = 'Produto cadastrado com sucesso!';
        this.novoProduto = { codigo: '', descricao: '', saldo: 0 };
        
        this.productService.refreshNeeded$.next();
        
        setTimeout(() => {
          this.showToast = false;
          this.cd.detectChanges();
        }, 3000);
      },
      error: (err: any) => {
        const mensagemErro = err.error?.message || err.error || 'Erro desconhecido ao cadastrar.';
        alert('Falha no cadastro: ' + mensagemErro);
      }
    });
  }

  faturar(prodInput: HTMLInputElement, qtdInput: HTMLInputElement): void {
    const payload = {
      status: 1, 
      itens: [
        {
          produtoId: Number(prodInput.value),
          quantidade: Number(qtdInput.value)
        }
      ]
    };

    this.http.post('https://localhost:7232/api/Invoice', payload).subscribe({
      next: (res: any) => { 
        prodInput.value = '';
        qtdInput.value = '';

        this.showToast = true;
        
        this.listInvoices();
        this.listProducts();

        setTimeout(() => {
          this.showToast = false;
          this.cd.detectChanges();
        }, 3000);
      },
      error: (err: any) => {
        console.error('Erro ao faturar:', err);
        
        this.errorMessage = err.error.error;
        this.showToastError = true;

        setTimeout(() => {
          this.showToastError = false;
          this.errorMessage = '';
          this.cd.detectChanges();
        }, 3000);
      }
    });
  }

  baixar(produto: Produto): void {
    this.productService.baixarEstoque(produto.id, 1).subscribe({
      next: () => {
        this.showToast = true;
        this.listProducts(); 

        setTimeout(() => {
          this.showToast = false;
          this.cd.detectChanges();
        }, 3000);
      },
      error: (err: any) => console.error("Erro ao baixar estoque", err)
    });
  }
}