# Rotas da API FnBook — referência para o frontend

Atualizado em **21/10/2026**, a partir dos controllers, DTOs e serviços deste repositório.
O catálogo contém **61 operações HTTP** em 12 controllers.

## Conexão

| Ambiente | URL base |
| --- | --- |
| Local, perfil `FnBook.API` de `launchSettings.json` | `https://localhost:62230` |
| Local, HTTP do mesmo perfil | `http://localhost:62231` (a aplicação usa redirecionamento para HTTPS) |
| Docker Compose | `http://localhost:5000` |

Use uma variável de ambiente do frontend para a URL base. Ela não inclui `/api`;
as rotas abaixo já incluem esse prefixo. Portas podem mudar quando sobrescritas na execução.
Em desenvolvimento, o Swagger está em `/swagger` e o documento OpenAPI em `/swagger/v1/swagger.json`.

Não há login/JWT implementado, exigência de token ou rotas de autenticação.
A API ainda não configura CORS em `Program.cs`; chamadas feitas por um frontend em
outra origem dependem de configurar CORS no backend ou um proxy de desenvolvimento.

## Convenções

- Requisições com corpo usam `Content-Type: application/json`; respostas JSON usam nomes em `camelCase`.
- Substitua `{id}`, `{newsId}`, `{userId}`, `{ufId}`, `{commentId}` e `{similarNewsId}` por GUIDs reais, sem chaves. GUID malformado no caminho não atende à rota e retorna `404`.
- Os GUIDs dos exemplos são ilustrativos. Campos que referenciam outros recursos devem usar os IDs retornados pelos respectivos cadastros.
- Datas são strings ISO 8601. Para enviar uma data, prefira UTC com `Z`, por exemplo `2026-10-21T15:00:00Z`.
- `GET` de coleção retorna um array direto, sem envelope, paginação, busca ou ordenação configurável. Sem resultados, retorna `200` com `[]`, inclusive nas rotas de filtro.
- `GET` por identificador retorna um objeto. `POST` bem-sucedido retorna `201`, o objeto criado e o header `Location`.
- `PUT` recebe o conjunto completo de campos editáveis. Campos omitidos podem ser substituídos por valores padrão; não trate `PUT` como atualização parcial.
- `DELETE` bem-sucedido retorna `204` sem corpo. Não execute `response.json()` nessa resposta.
- `string`, número, booleano e `null` nos exemplos representam os tipos JSON enviados. IDs são strings de GUID; datas também são strings.

## Disponibilidade atual

Categorias, curtidas de notícias e curtidas de comentários têm seus serviços e
repositórios registrados na injeção de dependências, assim como os demais recursos.

As tabelas precisam estar atualizadas pelas migrations. A API executa `MigrateAsync`
ao iniciar; instruções do banco estão no [README](README.md#banco-de-dados).

## Índice

- [Usuários](#usuarios)
- [UFs](#ufs)
- [Categorias](#categorias)
- [Notícias](#noticias)
- [Origens](#origens)
- [Comentários](#comentarios)
- [Denúncias de comentários](#denuncias)
- [Notificações](#notificacoes)
- [Fontes consultadas](#fontes)
- [Notícias similares](#similares)
- [Curtidas de notícias](#curtidas-noticias)
- [Curtidas de comentários](#curtidas-comentarios)
- [Erros e códigos HTTP](#erros-e-codigos-http)
- [Exemplo de consumo com fetch](#exemplo-de-consumo-com-fetch)

<a id="usuarios"></a>

## Usuários

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/Users` | Listar | `200` |
| `GET` | `/api/Users/{id}` | Consultar por ID | `200`, `404` |
| `POST` | `/api/Users` | Cadastrar | `201`, `400` |
| `PUT` | `/api/Users/{id}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/Users/{id}` | Excluir | `204`, `404` |

`POST` exige senha não vazia e e-mail ainda não cadastrado. E-mail em uso retorna `400`, inclusive no `PUT` de outro usuário. `userType` é uma string sem enumeração validada no serviço atual. A senha é enviada somente no cadastro; senha e hash não são retornados. `stateId` e `profilePicture` aceitam `null`. `createdAt` e `updatedAt` são definidos pelo servidor. `DELETE` exclui o registro; não é uma operação de desativação.

### Corpo do POST

```json
{
  "name": "Maria Silva",
  "email": "maria@example.com",
  "password": "senha-de-exemplo",
  "profilePicture": null,
  "stateId": "00000004-0000-0000-0000-000000000001",
  "userType": "usuario"
}
```

### Corpo do PUT

```json
{
  "name": "Maria Silva",
  "email": "maria@example.com",
  "profilePicture": null,
  "stateId": "00000004-0000-0000-0000-000000000001",
  "userType": "usuario"
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "id": "00000001-0000-0000-0000-000000000001",
  "name": "Maria Silva",
  "email": "maria@example.com",
  "profilePicture": null,
  "stateId": "00000004-0000-0000-0000-000000000001",
  "userType": "usuario",
  "contributorScore": 0,
  "isActive": true,
  "createdAt": "2026-10-21T15:00:00Z",
  "updatedAt": null
}
```

<a id="ufs"></a>

## UFs

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/Ufs` | Listar | `200` |
| `GET` | `/api/Ufs/{id}` | Consultar por ID | `200`, `404` |
| `POST` | `/api/Ufs` | Cadastrar | `201`, `400`, `409` |
| `PUT` | `/api/Ufs/{id}` | Atualizar | `200`, `400`, `404`, `409` |
| `DELETE` | `/api/Ufs/{id}` | Excluir | `204`, `404`, `409` |

`sigla` deve conter duas letras de A a Z; é aparada e convertida para maiúsculas. `nome` deve ter conteúdo e até 100 caracteres após remover espaços das extremidades. Sigla duplicada retorna `409`. A exclusão de uma UF vinculada a uma origem também retorna `409`. O identificador da resposta se chama **`ufId`**, embora o parâmetro da rota seja `{id}`.

### Corpo do POST e do PUT

```json
{
  "sigla": "SP",
  "nome": "São Paulo"
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "ufId": "00000004-0000-0000-0000-000000000001",
  "sigla": "SP",
  "nome": "São Paulo"
}
```

<a id="categorias"></a>

## Categorias

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/Categories` | Listar | `200` |
| `GET` | `/api/Categories/{id}` | Consultar por ID | `200`, `404` |
| `POST` | `/api/Categories` | Cadastrar | `201` |
| `PUT` | `/api/Categories/{id}` | Atualizar | `200`, `404` |
| `DELETE` | `/api/Categories/{id}` | Excluir | `204`, `404` |

O corpo de cadastro e atualização contém apenas `name`.

### Corpo do POST e do PUT

```json
{
  "name": "Política"
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "id": "00000005-0000-0000-0000-000000000001",
  "name": "Política"
}
```

<a id="noticias"></a>

## Notícias

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/News` | Listar | `200` |
| `GET` | `/api/News/{id}` | Consultar por ID | `200`, `404` |
| `POST` | `/api/News` | Cadastrar | `201`, `400` |
| `PUT` | `/api/News/{id}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/News/{id}` | Excluir | `204`, `404` |

`POST` inicializa `views` com zero; esse campo só é recebido no `PUT`. O `GET` não incrementa visualizações automaticamente. `aiTruth` e `finalTruth` são booleanos anuláveis: `null` representa ausência de classificação. `aiConfidence` é um número decimal anulável; `aiClassificationDate` e `verificationDate` são datas anuláveis. `image`, `categoryId`, `contentHash`, `createdBy`, `justification`, `aiModel`, `verifiedBy` e `embedding` também aceitam `null`. `embedding` é uma string, não um array numérico. Não há endpoints específicos para classificar com IA ou verificar notícias: os campos são recebidos pelo CRUD atual.

### Corpo do POST

```json
{
  "title": "Título da notícia",
  "text": "Texto completo da notícia",
  "image": null,
  "categoryId": "00000005-0000-0000-0000-000000000001",
  "contentHash": null,
  "createdBy": "00000001-0000-0000-0000-000000000001",
  "aiTruth": null,
  "justification": null,
  "aiModel": null,
  "aiConfidence": null,
  "aiClassificationDate": null,
  "finalTruth": null,
  "isVerified": false,
  "verifiedBy": null,
  "verificationDate": null,
  "embedding": null
}
```

### Corpo do PUT

```json
{
  "title": "Título da notícia",
  "text": "Texto completo da notícia",
  "image": null,
  "categoryId": "00000005-0000-0000-0000-000000000001",
  "contentHash": null,
  "createdBy": "00000001-0000-0000-0000-000000000001",
  "views": 12,
  "aiTruth": null,
  "justification": null,
  "aiModel": null,
  "aiConfidence": null,
  "aiClassificationDate": null,
  "finalTruth": null,
  "isVerified": false,
  "verifiedBy": null,
  "verificationDate": null,
  "embedding": null
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "id": "00000002-0000-0000-0000-000000000001",
  "title": "Título da notícia",
  "text": "Texto completo da notícia",
  "image": null,
  "categoryId": "00000005-0000-0000-0000-000000000001",
  "contentHash": null,
  "createdBy": "00000001-0000-0000-0000-000000000001",
  "views": 0,
  "aiTruth": null,
  "justification": null,
  "aiModel": null,
  "aiConfidence": null,
  "aiClassificationDate": null,
  "finalTruth": null,
  "isVerified": false,
  "verifiedBy": null,
  "verificationDate": null,
  "embedding": null
}
```

<a id="origens"></a>

## Origens

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/NewsOrigins` | Listar | `200` |
| `GET` | `/api/NewsOrigins/{id}` | Consultar por ID | `200`, `404` |
| `GET` | `/api/NewsOrigins/by-news/{newsId}` | Listar por notícia | `200` |
| `POST` | `/api/NewsOrigins` | Cadastrar | `201`, `400` |
| `PUT` | `/api/NewsOrigins/{id}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/NewsOrigins/{id}` | Excluir | `204`, `404` |

`userId`, `newsId`, `ufId` e `date` são obrigatórios. Usuário, notícia e UF precisam existir; referência inválida retorna `400`. A data é **fornecida pelo cliente** e também pode ser alterada no `PUT`. O identificador da resposta se chama **`origemId`**.

### Corpo do POST e do PUT

```json
{
  "userId": "00000001-0000-0000-0000-000000000001",
  "newsId": "00000002-0000-0000-0000-000000000001",
  "ufId": "00000004-0000-0000-0000-000000000001",
  "date": "2026-10-21T15:00:00Z"
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "origemId": "00000006-0000-0000-0000-000000000001",
  "userId": "00000001-0000-0000-0000-000000000001",
  "newsId": "00000002-0000-0000-0000-000000000001",
  "ufId": "00000004-0000-0000-0000-000000000001",
  "date": "2026-10-21T15:00:00Z"
}
```

<a id="comentarios"></a>

## Comentários

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/Comments` | Listar | `200` |
| `GET` | `/api/Comments/{id}` | Consultar por ID | `200`, `404` |
| `POST` | `/api/Comments` | Cadastrar | `201`, `400` |
| `PUT` | `/api/Comments/{id}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/Comments/{id}` | Excluir | `204`, `404` |

O comentário fica disponível imediatamente após o cadastro. `parentCommentId` aceita `null` para um comentário principal ou um GUID para uma resposta. A data é definida pelo servidor e preservada na edição. Não há campos de status, aprovação ou moderação. A remoção usa o `DELETE` do CRUD e exclui em cascata denúncias e notificações vinculadas. Não há rota específica para listar comentários por notícia: o frontend pode filtrar o resultado de `GET /api/Comments` por `newsId`.

### Corpo do POST e do PUT

```json
{
  "userId": "00000001-0000-0000-0000-000000000001",
  "newsId": "00000002-0000-0000-0000-000000000001",
  "parentCommentId": null,
  "text": "Meu comentário sobre a notícia."
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "id": "00000003-0000-0000-0000-000000000001",
  "userId": "00000001-0000-0000-0000-000000000001",
  "newsId": "00000002-0000-0000-0000-000000000001",
  "parentCommentId": null,
  "text": "Meu comentário sobre a notícia.",
  "date": "2026-10-21T15:00:00Z"
}
```

<a id="denuncias"></a>

## Denúncias de comentários

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/CommentReports` | Listar | `200` |
| `GET` | `/api/CommentReports/{id}` | Consultar por ID | `200`, `404` |
| `GET` | `/api/CommentReports/by-comment/{commentId}` | Listar por comentário | `200` |
| `POST` | `/api/CommentReports` | Cadastrar | `201`, `400` |
| `PUT` | `/api/CommentReports/{id}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/CommentReports/{id}` | Excluir | `204`, `404` |

Usuário e comentário precisam existir e seus GUIDs não podem ser vazios. `reason` é obrigatório e não aceita apenas espaços. A data é definida pelo servidor no cadastro e preservada na atualização. Registrar uma denúncia não exclui automaticamente o comentário; a exclusão é feita pelo CRUD de comentários.

### Corpo do POST e do PUT

```json
{
  "commentId": "00000003-0000-0000-0000-000000000001",
  "userId": "00000001-0000-0000-0000-000000000001",
  "reason": "Conteúdo ofensivo."
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "id": "00000007-0000-0000-0000-000000000001",
  "commentId": "00000003-0000-0000-0000-000000000001",
  "userId": "00000001-0000-0000-0000-000000000001",
  "reason": "Conteúdo ofensivo.",
  "date": "2026-10-21T15:00:00Z"
}
```

<a id="notificacoes"></a>

## Notificações

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/Notifications` | Listar | `200` |
| `GET` | `/api/Notifications/{id}` | Consultar por ID | `200`, `404` |
| `GET` | `/api/Notifications/by-user/{userId}` | Listar por usuário | `200` |
| `POST` | `/api/Notifications` | Cadastrar | `201`, `400` |
| `PUT` | `/api/Notifications/{id}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/Notifications/{id}` | Excluir | `204`, `404` |
| `PATCH` | `/api/Notifications/{id}/read` | Marcar como lida | `200`, `404` |

Usuário e comentário precisam existir. `type` é uma string obrigatória com até 50 caracteres; `resposta` é um exemplo, não uma enumeração fechada. `quantity` é inteiro de 1 até 2147483647, com padrão 1 quando omitido. `isRead` tem padrão `false`. `updatedAt` é definido pelo servidor ao cadastrar, atualizar ou marcar como lida. `PATCH /{id}/read` não recebe corpo e devolve a notificação completa com `isRead: true`. Notificações são cadastradas explicitamente; comentários e curtidas não geram notificações automaticamente.

### Corpo do POST e do PUT

```json
{
  "userId": "00000001-0000-0000-0000-000000000001",
  "commentId": "00000003-0000-0000-0000-000000000001",
  "type": "resposta",
  "quantity": 1,
  "isRead": false
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "id": "00000008-0000-0000-0000-000000000001",
  "userId": "00000001-0000-0000-0000-000000000001",
  "commentId": "00000003-0000-0000-0000-000000000001",
  "type": "resposta",
  "quantity": 1,
  "isRead": false,
  "updatedAt": "2026-10-21T15:00:00Z"
}
```

<a id="fontes"></a>

## Fontes consultadas

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/ConsultedSources` | Listar | `200` |
| `GET` | `/api/ConsultedSources/{id}` | Consultar por ID | `200`, `404` |
| `GET` | `/api/ConsultedSources/by-news/{newsId}` | Listar por notícia | `200` |
| `POST` | `/api/ConsultedSources` | Cadastrar | `201`, `400` |
| `PUT` | `/api/ConsultedSources/{id}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/ConsultedSources/{id}` | Excluir | `204`, `404` |

A notícia precisa existir. `url` é obrigatória, deve ser uma URL absoluta HTTP/HTTPS e ter até 2048 caracteres. `summary` é obrigatório e não aceita apenas espaços. O recurso não possui campo de data.

### Corpo do POST e do PUT

```json
{
  "newsId": "00000002-0000-0000-0000-000000000001",
  "url": "https://example.com/noticia",
  "summary": "Resumo da fonte consultada."
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "id": "00000009-0000-0000-0000-000000000001",
  "newsId": "00000002-0000-0000-0000-000000000001",
  "url": "https://example.com/noticia",
  "summary": "Resumo da fonte consultada."
}
```

<a id="similares"></a>

## Notícias similares

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/SimilarNews/by-news/{newsId}` | Listar por notícia | `200` |
| `GET` | `/api/SimilarNews/{newsId}/{similarNewsId}` | Consultar por ID | `200`, `404` |
| `POST` | `/api/SimilarNews` | Cadastrar | `201`, `400`, `409` |
| `PUT` | `/api/SimilarNews/{newsId}/{similarNewsId}` | Atualizar | `200`, `400`, `404` |
| `DELETE` | `/api/SimilarNews/{newsId}/{similarNewsId}` | Excluir | `204`, `404` |

As duas notícias precisam existir e devem ser distintas. `similarity` é um número decimal entre 0 e 1, inclusive. A chave é o par **`newsId` + `similarNewsId`**, sem `id` separado. Os pares são direcionais: A → B e B → A são registros diferentes. Repetir o mesmo par retorna `409`. O `PUT` altera somente `similarity`; a data é definida pelo servidor no cadastro e na atualização. Não há `GET /api/SimilarNews` para listar todas as relações; use `/by-news/{newsId}`.

### Corpo do POST

```json
{
  "newsId": "00000002-0000-0000-0000-000000000001",
  "similarNewsId": "00000010-0000-0000-0000-000000000001",
  "similarity": 0.85
}
```

### Corpo do PUT

```json
{
  "similarity": 0.9
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro, atualização e consulta.

```json
{
  "newsId": "00000002-0000-0000-0000-000000000001",
  "similarNewsId": "00000010-0000-0000-0000-000000000001",
  "similarity": 0.85,
  "date": "2026-10-21T15:00:00Z"
}
```

<a id="curtidas-noticias"></a>

## Curtidas de notícias

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/news-likes/noticia/{newsId}` | Listar por notícia | `200` |
| `POST` | `/api/news-likes` | Cadastrar | `201`, `409` |
| `DELETE` | `/api/news-likes/{newsId}/{userId}` | Excluir | `204`, `404` |

Cada curtida usa a chave composta `userId` + `newsId`. Duplicidade retorna `409`; não há `PUT`, identificador próprio ou rota para consultar todas as curtidas. No `DELETE`, o identificador da notícia vem **antes** do identificador do usuário. A listagem contém as curtidas, sem campo agregado de quantidade; conte os elementos do array para obter o total.

### Corpo do POST

```json
{
  "userId": "00000001-0000-0000-0000-000000000001",
  "newsId": "00000002-0000-0000-0000-000000000001"
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro e consulta.

```json
{
  "userId": "00000001-0000-0000-0000-000000000001",
  "newsId": "00000002-0000-0000-0000-000000000001"
}
```

<a id="curtidas-comentarios"></a>

## Curtidas de comentários

| Método | Rota | Ação | Respostas do controller |
| --- | --- | --- | --- |
| `GET` | `/api/comment-likes/comentario/{commentId}` | Listar por comentário | `200` |
| `POST` | `/api/comment-likes` | Cadastrar | `201`, `409` |
| `DELETE` | `/api/comment-likes/{commentId}/{userId}` | Excluir | `204`, `404` |

Cada curtida usa a chave composta `userId` + `commentId`. Duplicidade retorna `409`; não há `PUT`, identificador próprio ou rota para consultar todas as curtidas. No `DELETE`, o identificador do comentário vem **antes** do identificador do usuário. A listagem contém as curtidas, sem campo agregado de quantidade.

### Corpo do POST

```json
{
  "userId": "00000001-0000-0000-0000-000000000001",
  "commentId": "00000003-0000-0000-0000-000000000001"
}
```

### Objeto de resposta

O mesmo formato é usado nos itens das listagens e nas respostas de cadastro e consulta.

```json
{
  "userId": "00000001-0000-0000-0000-000000000001",
  "commentId": "00000003-0000-0000-0000-000000000001"
}
```

<a id="erros-e-codigos-http"></a>

## Erros e códigos HTTP

As tabelas de rotas mostram os retornos declarados pelos controllers. Erros de
JSON, conversão de tipos ou validação automática podem retornar `400` mesmo em
rotas que não declaram esse código. Exceções de persistência não tratadas podem
retornar `500`, por exemplo ao excluir registros protegidos por chave estrangeira;
não existe tratamento global que converta todas essas falhas em `400` ou `409`.

| Código | Tratamento no frontend |
| --- | --- |
| `200` | Ler o objeto ou array JSON. |
| `201` | Ler o objeto criado; `Location` aponta para uma consulta do recurso. |
| `204` | Operação concluída, sem corpo. |
| `400` | Conferir corpo, tipos e validações do recurso. E-mail duplicado também usa esse código. |
| `404` | Registro ausente, rota inexistente ou GUID inválido no caminho. |
| `409` | Conflito: UF duplicada/vinculada, curtida duplicada ou par de notícias similares repetido. |
| `500` | Falha interna; o corpo não tem um formato de erro estável. |

Erros de negócio tratados pelos controllers usam este formato (exemplo):

```json
{
  "error": "Usuário não encontrado."
}
```

Validação automática de `[ApiController]` pode retornar `ValidationProblemDetails`,
com mensagens em `errors`, por exemplo:

```json
{
  "type": "about:blank",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Reason": ["The Reason field is required."]
  },
  "traceId": "identificador da requisição"
}
```

Esse exemplo ilustra a estrutura: `type`, chaves, mensagens e `traceId` variam.
O frontend deve aceitar tanto `error` quanto `errors`, além de respostas sem JSON.

<a id="exemplo-de-consumo-com-fetch"></a>

## Exemplo de consumo com fetch

```js
const API_BASE_URL = "https://localhost:62230";

async function apiRequest(path, { method = "GET", body } = {}) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    method,
    headers: {
      Accept: "application/json",
      ...(body !== undefined ? { "Content-Type": "application/json" } : {}),
    },
    ...(body !== undefined ? { body: JSON.stringify(body) } : {}),
  });

  if (response.status === 204) return null;

  const text = await response.text();
  let data = null;
  if (text) {
    try { data = JSON.parse(text); }
    catch { data = text; }
  }

  if (!response.ok) {
    const validation = data?.errors
      ? Object.values(data.errors).flat().join("; ")
      : null;
    const message = data?.error || validation || data?.title
      || `Falha na API: HTTP ${response.status}`;
    const error = new Error(message);
    error.status = response.status;
    error.data = data;
    throw error;
  }

  return data;
}

const comments = await apiRequest("/api/Comments");
// Use um ID real retornado pela API:
// await apiRequest(`/api/Comments/${commentId}`, { method: "DELETE" });
// await apiRequest(`/api/Notifications/${notificationId}/read`, { method: "PATCH" });
```

Para manter este guia, confira os contratos em
[`Controllers`](src/FnBook.API/Controllers),
[`DTOs`](src/FnBook.Application/DTOs) e
[`Services`](src/FnBook.Application/Services).
