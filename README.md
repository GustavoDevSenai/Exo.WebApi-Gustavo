# Exo.WebApi

API desenvolvida em **ASP.NET Core** para gerenciamento de projetos da empresa ExoApi.

## Tecnologias utilizadas

- ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- SQL Server
- Swagger

## Funcionalidades

- Cadastro de projetos
- Listagem de projetos
- Persistência de dados utilizando SQL Server

## Estrutura do projeto

```
Exo.WebApi
│
├── Contexts        # Contexto do Entity Framework
├── Controllers     # Endpoints da API
├── Models          # Classes de domínio
├── Repositories    # Camada de acesso aos dados
├── Properties      # Configurações da aplicação
├── Program.cs      # Configuração da API
└── Exo.WebApi.csproj
```

## Como executar o projeto

### 1. Clonar o repositório

```bash
git clone URL_DO_REPOSITORIO
```

### 2. Acessar a pasta

```bash
cd Exo.WebApi
```

### 3. Restaurar dependências

```bash
dotnet restore
```

### 4. Executar a aplicação

```bash
dotnet run
```

A API estará disponível através da URL informada no terminal.

## Banco de dados

A aplicação utiliza SQL Server.

Configure a string de conexão no arquivo:

```
Contexts/ExoContext.cs
```

Exemplo:

```csharp
Server=localhost\SQLEXPRESS;
Database=ExoApi;
Trusted_Connection=True;
TrustServerCertificate=True;
```

## Documentação da API

Após executar o projeto, acesse:

```
/swagger
```

para visualizar e testar os endpoints disponíveis.

## Autor

Gustavo Rodrigues
