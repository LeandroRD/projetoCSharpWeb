# ProjetoCSharpWeb

Este projeto é um clone do sistema PHP/CodeIgniter, desenvolvido em ASP.NET Core MVC com integração ao MySQL do Laragon.

## Funcionalidades
- Cadastro, listagem, edição e exclusão de usuários
- Cadastro, listagem, edição e exclusão de fornecedores
- Integração total com o banco MySQL já existente

## Como rodar
1. Configure a string de conexão com o MySQL no arquivo `appsettings.json`.
2. Execute `dotnet run` na pasta do projeto.
3. Acesse `http://localhost:5000` no navegador.

## Observações
- O banco de dados MySQL deve estar rodando (pode ser o mesmo do projeto PHP).
- O projeto utiliza o pacote Pomelo.EntityFrameworkCore.MySql para integração com o MySQL.
