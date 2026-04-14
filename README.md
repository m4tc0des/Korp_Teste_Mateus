# Sistema de Gestão de Estoque e Faturamento (Desafio Korp)

Este projeto consiste em um ecossistema de gestão de estoque e faturamento, estruturado em **Microsserviços** para garantir escalabilidade, independência de dados e separação de responsabilidades.

## Tecnologias Utilizadas

### **Backend**
* **.NET 8 / C#**
* **Arquitetura:** Clean Architecture, Domain-Driven Design (DDD) e Princípios SOLID.
* **Banco de Dados:** MySQL (Instâncias independentes para cada serviço para garantir o desacoplamento).
* **Comunicação:** HTTP Client (Comunicação síncrona via `IHttpClientFactory`).
* **ORM:** Entity Framework Core com Pomelo MySQL Provider.

### **Frontend**
* **Angular 19**
* **Estado e Reatividade:** RxJS (Uso de `Subject` para atualização de lista em tempo real sem Refresh).
* **Interface:** Design responsivo e focado em UX (Single Screen View).

---

## Arquitetura e Microsserviços

O sistema é dividido em dois serviços independentes e um portal web:

1.  **Serviço de Estoque:** Responsável pelo cadastro de produtos e controle rigoroso de saldos. Gerencia o banco de dados `EstoqueDB`.
2.  **Serviço de Faturamento:** Gestão de Notas Fiscais e emissão de numeração sequencial. Comunica-se com o Estoque via API para validar e baixar produtos. Gerencia o banco `FaturamentoDB`.
3.  **Portal Web (Angular):** Interface única para gerenciamento, consumo de APIs e feedback visual via Toasts.

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
   
