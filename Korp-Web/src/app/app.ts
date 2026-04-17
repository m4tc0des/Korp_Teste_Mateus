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
  public showToastError = false;
  public errorMessage = '';
  public processando = false;

  public itensNoRascunho: any[] = [];
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
        console.error("Erro ao carregar produtos:", err);
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
      error: (err: any) => console.error("Erro ao carregar faturamento:", err)
    });
  }

  salvarProduto(): void {
    if (!this.novoProduto.codigo || !this.novoProduto.descricao) {
      this.exibirErro('Preencha os campos obrigatórios!');
      return;
    }

    const produtoParaEnviar = {
      Codigo: this.novoProduto.codigo,
      Descricao: this.novoProduto.descricao,
      Saldo: this.novoProduto.saldo
    };

    this.productService.cadastrarProduto(produtoParaEnviar).subscribe({
      next: (res: any) => {
        this.novoProduto = { codigo: '', descricao: '', saldo: 0 };
        
        this.listProducts(); 
        
        this.showToast = true;
        this.cd.detectChanges();

        setTimeout(() => { 
          this.showToast = false; 
          this.cd.detectChanges(); 
        }, 3000);
      },
      error: (err: any) => {
        const msg = err.error?.error || 'Falha no cadastro.';
        this.exibirErro(msg);
      }
    });
  }

  adicionarAoRascunho(prodIdInput: HTMLInputElement, qtdInput: HTMLInputElement) {
    const id = parseInt(prodIdInput.value);
    const quantidade = parseInt(qtdInput.value);

    if (!id || !quantidade || quantidade <= 0) {
      this.exibirErro("Informe um ID válido e uma quantidade maior que zero.");
      return;
    }

    const produtoNoEstoque = this.products.find(p => p.id === id);

    if (!produtoNoEstoque) {
      this.exibirErro(`Produto com ID ${id} não encontrado no estoque.`);
      return;
    }

    if (quantidade > produtoNoEstoque.saldo) {
      this.exibirErro(`Saldo insuficiente! "${produtoNoEstoque.descricao}" tem apenas ${produtoNoEstoque.saldo} un.`);
      return;
    }

    this.itensNoRascunho.push({
      produtoId: id,
      quantidade: quantidade,
      descricao: produtoNoEstoque.descricao, 
      produtoCodigo: produtoNoEstoque.codigo
    });

    prodIdInput.value = '';
    qtdInput.value = '';
    this.cd.detectChanges();
  }

  gerarNotaFiscal(): void {
    if (this.itensNoRascunho.length === 0) {
      this.exibirErro("Adicione ao menos um produto no rascunho.");
      return;
    }

    this.processando = true;

    const payload = {
      itens: this.itensNoRascunho.map(i => ({
        produtoId: i.produtoId,
        quantidade: i.quantidade
      }))
    };

    this.http.post('https://localhost:7232/api/invoices', payload).subscribe({
      next: (res: any) => { 
        this.showToast = true;
        this.itensNoRascunho = []; 
        
        this.listInvoices();
        this.listProducts(); 
        
        this.processando = false;
        this.cd.detectChanges();
        
        setTimeout(() => {
          this.showToast = false;
          this.cd.detectChanges();
        }, 3000);
      },
      error: (err: any) => {
        this.processando = false;
        this.exibirErro(err.error?.error || 'Erro ao gerar nota fiscal.');
      }
    });
  }

  faturarNota(notaId: number): void {
    this.processando = true;
    this.http.post(`https://localhost:7232/api/invoices/fecharNota/${notaId}`, {}).subscribe({
      next: (res: any) => {
        this.showToast = true;
        
        this.listInvoices();
        this.listProducts(); 
        
        this.processando = false;
        this.cd.detectChanges();
        
        setTimeout(() => {
          this.showToast = false;
          this.cd.detectChanges();
        }, 3000);
      },
      error: (err: any) => {
        this.processando = false;
        this.exibirErro(err.error?.error || 'Erro ao faturar nota.');
      }
    });
  }

  imprimirNota(nota: any) {
    const dataEmissao = new Date().toLocaleString();
    const printWindow = window.open('', '_blank');
    
    if (printWindow) {
      printWindow.document.write(`
        <html>
          <head>
            <title>Nota Fiscal #${nota.numeroSequencial}</title>
            <style>
              body { font-family: sans-serif; padding: 20px; color: #333; }
              .header { border-bottom: 2px solid #2980b9; padding-bottom: 10px; margin-bottom: 20px; }
              table { width: 100%; border-collapse: collapse; margin-top: 20px; }
              th, td { border: 1px solid #ddd; padding: 10px; text-align: left; }
              th { background-color: #f4f7f6; }
              .footer { margin-top: 30px; font-size: 0.8em; color: #7f8c8d; text-align: center; }
            </style>
          </head>
          <body>
            <div class="header">
              <h1>NOTA FISCAL DE VENDA</h1>
              <p><strong>Nº Sequencial:</strong> #${nota.numeroSequencial}</p>
              <p><strong>Data de Emissão:</strong> ${dataEmissao}</p>
              <p><strong>Status:</strong> ${nota.status === 1 ? 'ABERTA' : 'FECHADA'}</p>
            </div>
            <h3>ITENS DA NOTA</h3>
            <table>
              <thead>
                <tr>
                  <th>Cód. Produto</th>
                  <th>Quantidade</th>
                </tr>
              </thead>
              <tbody>
                ${nota.itens.map((item: any) => `
                  <tr>
                    <td>${item.produtoCodigo} (ID: ${item.produtoId})</td>
                    <td>${item.quantidade}x</td>
                  </tr>
                `).join('')}
              </tbody>
            </table>
            <div class="footer">
              <p>Emitido por Sistema Korp - Desenvolvedor: Mateus Silva</p>
            </div>
            <script>
              window.onload = function() { window.print(); window.close(); }
            </script>
          </body>
        </html>
      `);
      printWindow.document.close();
    }
  }

  exibirErro(msg: string): void {
    this.errorMessage = msg;
    this.showToastError = true;
    this.cd.detectChanges();

    setTimeout(() => {
      this.showToastError = false;
      this.errorMessage = '';
      this.cd.detectChanges();
    }, 7000);
  }
}