# Sistema de Gestão de Estoque e Faturamento (Desafio Korp)

Este projeto consiste em um ecossistema de gestão de estoque e faturamento, estruturado em Microsserviços para garantir escalabilidade, independência de dados e separação de responsabilidades.

Diferenciais Implementados nesta Versão
Validação Cross-Service: O Faturamento agora valida o saldo em tempo real consultando o microserviço de Estoque via HTTP antes de emitir qualquer nota.

Programação Defensiva: Implementação de travas no Backend e Frontend para impedir estados inválidos (ex: venda de produto esgotado).

UX Aprimorada: Feedback visual dinâmico via RxJS e tratamento de erros global que traduz exceções do backend para alertas amigáveis no frontend.

## Tecnologias Utilizadas

### **Backend**
* **.NET 8 / C#**
* **Arquitetura:** Clean Architecture, Domain-Driven Design (DDD) e Princípios SOLID.
* **Banco de Dados:** MySQL (Instâncias independentes para cada serviço para garantir o desacoplamento).
* **Comunicação:** HTTP Client (Comunicação síncrona via `IHttpClientFactory`).
* **ORM:** Entity Framework Core com Pomelo MySQL Provider.

### **Frontend**
* **Angular 19**
* **Estado e Reatividade:** RxJS (Uso de `Subject` e `Observable` para sincronização de listas sem necessidade de refresh manual).
* **Interface:** Design responsivo e focado em UX (Single Screen View).

---

## Arquitetura e Microsserviços

O sistema é dividido em dois serviços independentes e um portal web:

Serviço de Estoque: Gerencia o ciclo de vida do produto e garante a integridade do saldo no banco EstoqueDB.

Serviço de Faturamento: Responsável pela emissão de Notas Fiscais. Regra de Ouro: Não emite notas sem confirmação de saldo positivo via integração síncrona com o Estoque. Gerencia o banco FaturamentoDB.

Portal Web (Angular): Interface unificada que consome ambos os serviços, tratando falhas de comunicação de forma graciosa.

---

## Detalhamento Técnico

### **Integração Front-End & Back-End**
* **CORS:** Configurado para permitir comunicações seguras do ambiente de desenvolvimento.
* **Padronização JSON:** Implementado `JsonNamingPolicy.CamelCase` para compatibilidade nativa entre C# e TypeScript.
* **Tratamento de Erros:** Middleware para captura de exceções e feedback amigável ao usuário.

### **Estrutura de Pastas (Clean Architecture)**
* `Application`: Regras de aplicação, DTOs e Mapeamentos.
* `Domain`: Entidades core, Interfaces e Regras de Negócio.
* `Infrastructure`: Repositórios, Migrations e Contexto de Dados.
* `API`: Controllers e Injeção de Dependência.

---

## ⚙️ Como rodar o projeto

1. **Banco de Dados:** Configure as strings de conexão nos arquivos `appsettings.json` de cada API.
2. **Migrações:** * No terminal do serviço de Estoque: `dotnet ef database update`
   * No terminal do serviço de Faturamento: `dotnet ef database update`
3. **Backend:** No Visual Studio, defina os dois projetos (Estoque e Faturamento) para iniciar simultaneamente e pressione `F5`.
4. **Frontend:**
   ```bash
   cd Korp-Web
   npm install
   ng serve
   ```
5. **Acesse: http://localhost:4200**
   
