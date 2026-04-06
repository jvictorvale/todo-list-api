# 📝 To-do List API (.NET 6)

[![.NET 6](https://img.shields.io/badge/.NET-6.0-512bd4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/6.0)
[![MySQL](https://img.shields.io/badge/MySQL-00758f?style=flat-square&logo=mysql&logoColor=white)](https://www.mysql.com/)
[![JWT](https://img.shields.io/badge/JWT-black?style=flat-square&logo=json-web-tokens)](https://jwt.io/)

Esta é uma API RESTful robusta desenvolvida para o gerenciamento individual de tarefas. O sistema foca em **segurança avançada** e **isolamento de dados**, garantindo que cada usuário gerencie apenas seu próprio conteúdo através de autenticação via Token.

## 🏗️ Arquitetura do Projeto

O projeto segue a separação de responsabilidades em 4 camadas principais:
- **API:** Controllers e configuração de Injeção de Dependência.
- **Application:** Serviços, DTOs e lógica de orquestração.
- **Domain:** Entidades, Interfaces e regras de negócio centrais.
- **Infrastructure:** Acesso ao banco de dados (EF Core), Mappings e Migrations.

## 🛡️ Destaques Técnicos & Segurança

* **Hashing Argon2:** Utilização do algoritmo `ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher`, garantindo proteção de alto nível contra ataques de força bruta.
* **Autenticação JWT:** Proteção de endpoints e extração de identidade do usuário via Claims.
* **Isolamento de Tenant:** Lógica de negócio que impede o acesso transversal entre tarefas de diferentes usuários.
* **Clean Code:** Uso de **AutoMapper** para conversão de objetos e **FluentValidation** para regras de entrada limpas.

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core Web API** (.NET 6)
- **Entity Framework Core** (ORM)
- **MySQL** (Banco de Dados)
- **Swagger** (Documentação da API)

## ⚙️ Como Executar

1. Clone o repositório.
2. Configure a `DefaultConnection` no seu `appsettings.json` apontando para o seu MySQL.
3. Execute o comando de migração:
   ```bash
   dotnet ef database update
