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
dotnet tool restore
dotnet run --project src/FnBook.API
```

## Rodar via Docker

```bash
docker compose up --build -d
```

API em `http://localhost:5000` (container escuta em 8080). Swagger em `http://localhost:5000/swagger`.

O banco SQLite fica em `./data/fnbook.db`, montado como volume em `/app/data`.
A API aplica as migrações do EF Core ao iniciar. Para criar novas migrações, execute:

```bash
dotnet tool restore
dotnet ef migrations add NomeDaMudanca --project src/FnBook.Infrastructure --startup-project src/FnBook.API --output-dir Data/Migrations
```

Se você já tinha um banco de testes criado com `EnsureCreated`, faça uma cópia se quiser guardar
os dados e remova os arquivos `fnbook.db`, `fnbook.db-wal` e `fnbook.db-shm` com a API parada.
Na próxima inicialização, as migrações criarão o banco novamente.

## Parar

```bash
docker compose down
```

## Pendências

Baseado na modelagem (`Modelagem dos dados.jpeg`), falta implementar:

- Uf (em andamento, outro integrante)
- Origem (em andamento, outro integrante)
- Denuncia_Comentario
- Notificacao
- Fonte_Consultada
- Noticia_Similar
- Moderação de Comentario (endpoints de aprovar/rejeitar)
- Hash de senha real (hoje é só Base64, não é hash)
- Autenticação/login (JWT)
- Migrations do EF Core (hoje usa `EnsureCreated`)
