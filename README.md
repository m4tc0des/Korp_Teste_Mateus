# Sistema de Emissão de Notas Fiscais (Desafio Korp)
Este projeto consiste em um sistema de gestão de estoque e faturamento, estruturado em microsserviços para garantir escalabilidade, independência de dados e separação de responsabilidades.

# Tecnologias Utilizadas
Backend
.NET Core 8 / C#

Arquitetura: Domain-Driven Design (DDD), Clean Architecture & Microservices.

Banco de Dados: MySQL (Instâncias independentes para cada serviço).

Comunicação: HTTP Client (Comunicação síncrona entre serviços via IHttpClientFactory).

Bibliotecas: Entity Framework Core, Pomelo (MySQL Provider), Mapster e Swagger.

Frontend
Angular 19

Estilização: SCSS e Design Responsivo.

Consumo de API: RxJS e HttpClient.

# Arquitetura e Microsserviços
O sistema é dividido em dois microsserviços independentes e um portal web:

Serviço de Estoque: Cadastro de produtos e controle rigoroso de saldos. Possui banco próprio (EstoqueDB).

Serviço de Faturamento: Gestão de Notas Fiscais e fechamento. Comunica-se com o Estoque via API para validar e baixar produtos no banco (FaturamentoDB).

Portal Web (Frontend): Interface em Angular para visualização de estoque em tempo real.

# Detalhamento Técnico (Backend)
Integração Front-End & Back-End
CORS: Configurado no backend para permitir comunicações seguras vindas do http://localhost:4200.

Padronização JSON: Implementado JsonNamingPolicy.CamelCase no .NET para garantir que o Angular consuma os dados de forma nativa e eficiente.

Estrutura de Projetos (Backend)
API: Controllers e Injeção de Dependência.

Application: Serviços de aplicação e DTOs.

Domain: Entidades de negócio, Interfaces e Regras de Ouro.

Infrastructure: Repositórios e Contextos do EF Core.

# Como rodar o projeto
1. Banco de Dados
Certifique-se de que o MySQL está ativo e configure as strings de conexão nos arquivos appsettings.json de cada API:

Estoque.API -> Banco de Estoque.

Faturamento.API -> Banco de Faturamento.

2. Execução das Migrações
No Package Manager Console ou Terminal, rode:

Estoque: Update-Database -Context EstoqueDbContext

Faturamento: Update-Database -Context FaturamentoContext

3. Inicialização do Backend
Abra a solução no Visual Studio.

Configure Múltiplos projetos de inicialização: Defina Estoque.API e Faturamento.API como "Iniciar".

Pressione F5.

4. Inicialização do Frontend (Angular)
Navegue até a pasta do frontend: cd Korp-Web

Instale as dependências: npm install

Rode o projeto: ng serve

Acesse: http://localhost:4200

# Vídeo de Demonstração
// A ser construído
