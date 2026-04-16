export interface InvoiceItem {
    produtoId: number;
    produtoCodigo: string;
    quantidade: number;
}

export interface Invoice {
    id?: number;
    numeroSequencial: string;
    itens: InvoiceItem[];
}