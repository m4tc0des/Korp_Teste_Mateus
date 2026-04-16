export interface InvoiceItem {
    produtoId: number;
    produtoCodigo: string;
    quantidade: number;
}

export interface Invoice {
  id: number;
  numeroSequencial: number;
  status: number;
  itens: any[];
}   