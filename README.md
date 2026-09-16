# FnBook — Backend

API .NET 10 em Clean Architecture (Domain / Application / Infrastructure / API), persistência SQLite.

## Estrutura

```
src/
  FnBook.Domain/          entidades e interfaces de repositório
  FnBook.Application/     DTOs, serviços, mapeamentos
  FnBook.Infrastructure/  EF Core, DbContext, repositórios
  FnBook.API/             controllers e composition root
```

## Requisitos

- .NET SDK 10.0
- Docker Desktop (opcional, para rodar em container)

## Rodar local (host)

```bash
dotnet restore FnBook.sln
dotnet build FnBook.sln
dotnet run --project src/FnBook.API
```

## Rodar via Docker

```bash
docker compose up --build -d
```

API em `http://localhost:5000` (container escuta em 8080). Swagger em `http://localhost:5000/swagger`.

O banco SQLite fica em `./data/fnbook.db`, montado como volume em `/app/data`.

## Parar

```bash
docker compose down
```
