# Sistema de Emissão de Notas Fiscais (Desafio Korp)
Este projeto consiste em um sistema de gestão de estoque e faturamento, estruturado em microsserviços para garantir escalabilidade, independência de dados e separação de responsabilidades.

# Tecnologias Utilizadas
Backend: .NET Core 8 / C#

Arquitetura: Domain-Driven Design (DDD), Clean Architecture & Microservices

Banco de Dados: MySQL (Instâncias independentes para cada serviço)

Comunicação: HTTP Client (Comunicação síncrona entre serviços)

Bibliotecas Principais:

Entity Framework Core: ORM para persistência e Migrations.

Pomelo.EntityFrameworkCore.MySql: Provider para integração com MySQL.

Mapster: Mapeamento entre Entidades e DTOs de alta performance.

Swagger: Documentação e testes das APIs.

# Arquitetura e Microsserviços
O sistema foi dividido em dois microsserviços independentes para atender aos requisitos de escalabilidade e desacoplamento:

Serviço de Estoque: Responsável pelo cadastro de produtos e controle rigoroso de saldos. Possui seu próprio banco de dados (EstoqueDB).

Serviço de Faturamento: Gestão de Notas Fiscais, controle de numeração sequencial e fechamento de notas. Possui seu próprio banco de dados (FaturamentoDB) e comunica-se com o Estoque via API para validar e baixar produtos.

# Detalhamento Técnico (Backend)
Estrutura de Projetos
A solução segue os princípios da Clean Architecture, garantindo que a regra de negócio não dependa de frameworks externos:

API: Porta de entrada, configuração de Injeção de Dependência e Controllers.

Application: Serviços de aplicação, orquestração de chamadas e DTOs.

Domain: Entidades de negócio, Enums, Interfaces e o "Coração" do sistema.

Infrastructure: Implementação de repositórios, Contextos do EF Core e Migrations.

Comunicação entre Microsserviços
Foi implementado um padrão de comunicação via HTTP utilizando IHttpClientFactory. Quando uma nota é fechada no Serviço de Faturamento, ele dispara uma requisição para o Serviço de Estoque para validar a existência do item e realizar a baixa do saldo de forma atômica.

Tratamento de Erros e Exceções
Uso de Middleware global para captura de exceções, garantindo que o cliente receba respostas padronizadas mesmo em caso de falhas internas.
Validações de negócio (ex: Saldo Insuficiente ou Produto Inexistente) são tratadas para retornar códigos de status HTTP apropriados (como 400 Bad Request ou 404 Not Found).

# Como rodar o projeto
Configuração do Banco de Dados

Certifique-se de que o MySQL está em execução em sua máquina.

No arquivo appsettings.json do Estoque.API, configure a string de conexão para o banco de estoque.

No arquivo appsettings.json do Faturamento.API, configure a string de conexão para o banco de faturamento.

Execução das Migrações
No Terminal ou Package Manager Console, execute os comandos para criar os bancos:

Para o Estoque:
Update-Database -Context EstoqueDbContext

Para o Faturamento:
Update-Database -Context FaturamentoContext

Inicialização

Abra a solução no Visual Studio.

Clique com o botão direito na Solução > Configurar Projetos de Inicialização.

Selecione "Múltiplos projetos de inicialização".

Defina como "Iniciar" os projetos Estoque.API e Faturamento.API.

Pressione F5 para rodar ambos simultaneamente.

# Vídeo de Demonstração
// A ser construído
