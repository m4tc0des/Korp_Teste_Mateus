# Sistema de Emissão de Notas Fiscais (Desafio Korp)
Este projeto consiste em um sistema de gestão de estoque e faturamento, estruturado em microsserviços para garantir escalabilidade e separação de responsabilidades.

# Tecnologias Utilizadas
Frontend: Angular // A ser construído

Backend: .NET Core 8 / C#

Arquitetura: Domain-Driven Design (DDD) & Clean Architecture

Banco de Dados: MySQL (Persistência Real)

Bibliotecas Principais:

Entity Framework Core: ORM para persistência.

Mapster: Mapeamento entre Entidades e DTOs (Alta performance).

Swagger: Documentação de API.

RxJS (Frontend): // A ser construído

# Arquitetura e Microsserviços
O sistema foi dividido em dois microsserviços principais para atender aos requisitos de escalabilidade e desacoplamento:

Serviço de Estoque: Responsável pelo cadastro de produtos e controle rigoroso de saldos.

Serviço de Faturamento: Gestão de Notas Fiscais, controle de numeração sequencial e fechamento de notas.

# Detalhamento Técnico (Backend)
Frameworks e Padrões
Foi utilizado o framework ASP.NET Core com C#. A solução segue os princípios da Clean Architecture, dividida em:

Estoque.API: Porta de entrada (Controllers).

Estoque.Application: Orquestração e DTOs.

Estoque.Domain: Entidades de negócio e interfaces (Coração do sistema).

Estoque.Infrastructure: Implementação de repositórios e acesso ao banco.

Uso de LINQ
O LINQ foi utilizado de forma extensiva nos repositórios para consultas otimizadas e filtragem de dados no banco, garantindo um código limpo e performático.

Tratamento de Erros e Exceções
Implementação de um Middleware global de exceções para capturar falhas inesperadas e retornar um feedback padronizado em JSON ao usuário.

Validações de domínio (ex: saldo insuficiente) lançam exceções específicas que são tratadas para fornecer mensagens amigáveis no frontend.

# Detalhamento Técnico (Frontend - Angular)
// A ser construído

# Como rodar o projeto
Banco de Dados
Configure a string de conexão no arquivo appsettings.json do projeto Estoque.API.

No console do Gerenciador de Pacotes, execute: Update-Database.

Backend
Abra a solução EstoqueService.sln no Visual Studio.

Defina os projetos de inicialização e execute.

Frontend
// A ser construído

# Vídeo de Demonstração
// A ser construído
