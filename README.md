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

## Funcionalidades implementadas

As entidades seguem os campos de `Modelagem dos dados.jpeg`, com DTOs, mapeamentos,
serviços, repositórios, configuração do EF Core e controllers.

| Recurso | Rota base | Operações |
| --- | --- | --- |
| Denuncia_Comentario | `/api/CommentReports` | GET, GET por ID, POST, PUT por ID, DELETE por ID; GET `/by-comment/{commentId}` |
| Notificacao | `/api/Notifications` | GET, GET por ID, POST, PUT por ID, DELETE por ID; GET `/by-user/{userId}`; PATCH `/{id}/read` |
| Fonte_Consultada | `/api/ConsultedSources` | GET, GET por ID, POST, PUT por ID, DELETE por ID; GET `/by-news/{newsId}` |
| Noticia_Similar | `/api/SimilarNews` | GET `/by-news/{newsId}`, GET/PUT/DELETE `/{newsId}/{similarNewsId}`, POST |

Os novos recursos validam a existência dos usuários, comentários e notícias referenciados.
Campos de data são definidos pelo servidor em UTC. Fontes aceitam URLs HTTP/HTTPS.
Notícias similares usam chave composta, similaridade de 0 a 1 e duas notícias distintas;
um par já cadastrado retorna HTTP 409. Os pares são direcionais, conforme a chave da modelagem.
Notificações são cadastradas e atualizadas explicitamente pela API.

### Comentários

Comentários são publicados imediatamente ao serem cadastrados, sem aprovação prévia.
O CRUD está disponível em `/api/Comments`: GET e POST na rota base;
GET, PUT e DELETE em `/{id}`. A edição preserva a data de criação.
Para remover um comentário, use `DELETE /api/Comments/{id}`: retorna 204 ao excluir
ou 404 se o comentário não existir. Denúncias e notificações vinculadas são removidas
em cascata. Não há status ou campos de moderação nos DTOs e na entidade de comentário.

### Senhas

Novos cadastros usam o `PasswordHasher` do ASP.NET Identity (PBKDF2, salt aleatório,
210.000 iterações), com formato versionado e serviço de verificação disponível para
o futuro login. A senha e seu hash não são expostos nos DTOs de resposta.
Senhas vazias são rejeitadas. Valores antigos em Base64 não são convertidos nem
aceitos pelo novo verificador; o tratamento dessas contas fica para a implementação do login.

### Banco de dados

As configurações das quatro novas entidades estão no `AppDbContext`, mas as migrations
foram deixadas fora desta entrega. O banco existente precisa de uma migration que crie
as novas tabelas e remova as antigas colunas de moderação de `Comentario`. A inicialização continua usando
`MigrateAsync`; com o modelo diferente do snapshot, o EF Core pode impedir a inicialização
até essa etapa ser realizada. Nenhum arquivo de migration ou banco existente foi alterado.

### Validação

```bash
dotnet build FnBook.sln
dotnet run --project tests/FnBook.IntegrationTests
```

Os testes de integração são um executável sem dependências adicionais de framework
de testes. Usam SQLite em memória, criado apenas para a execução, e verificam CRUD,
filtros, referências, publicação e exclusão de comentários, respostas dos controllers e hash de senha.
Falhas encerram o processo com erro, sem alterar o banco da aplicação.

## Pendências

- Uf (em andamento, outro integrante)
- Origem (em andamento, outro integrante)
- Autenticação/login (JWT)
- Migrations do EF Core para as novas entidades (fora do escopo desta entrega)
