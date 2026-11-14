# Grana Amiga — API de Conteúdo Educativo (.NET)

## Integrantes

| Nome | RM | Turma |
|---|---|---|
| Matheus Peres Moraes de Jesus | RM567300 | 2TDSPR |
| Cassio Ribeiro | RM567295 | 2TDSPR |
| Davi Isac Colin Pereira | RM567265 | 2TDSPR |
| Hugo Souza de Jesus | RM568542 | 2TDSPR |
| Pedro Gonçalves Rodrigues | RM567651 | 2TDSPR |

## Introdução

O endividamento deixou de ser exceção nos lares brasileiros e passou a fazer parte da rotina. Em julho de 2026, 82% das famílias tinham alguma dívida, o maior patamar da série histórica da Pesquisa de Endividamento e Inadimplência do Consumidor (Peic/CNC). Entre as que vivem com até três salários mínimos, o índice chega a 84,9%, e o cartão de crédito, com o rotativo e o parcelamento, aparece como a principal origem do problema. Para essas famílias, a dívida raramente nasce de uma única decisão ruim. Ela se forma no acúmulo de pequenos gastos que ninguém registra, de juros que ninguém calculou e de contas, como a de luz, que só ficam visíveis quando chegam.

A maior parte das ferramentas de finanças pessoais parte do pressuposto de que o usuário tem tempo, familiaridade com planilhas, internet estável e conhecimento prévio sobre juros. Esse não é o retrato de quem mais precisa de ajuda. É nesse espaço que surge o **Grana Amiga**, uma CitizenTech de educação e organização financeira pensada para famílias de baixa renda, com linguagem simples, ícones grandes, recursos de áudio e funcionamento mesmo com conectividade limitada.

A solução se apoia em dois movimentos complementares: aprender e organizar. Na área aberta, sem cadastro, qualquer pessoa joga simuladores curtos sobre juros, compras do dia a dia e poupança, organizados em trilhas por tema e nível de dificuldade. Ao criar uma conta, a família ganha um painel com o **Termômetro do Dinheiro**, que classifica a situação financeira em Verde, Amarelo ou Vermelho conforme o comprometimento da renda, e um plano visual de quitação que prioriza as dívidas com os juros mais altos. Os gastos podem ser registrados por ícones, por voz ou pelo WhatsApp. Um medidor de energia de baixo custo, baseado em ESP32, traduz o consumo elétrico da casa em reais e avisa antes que a conta ultrapasse a meta do mês, transformando um gasto invisível em uma decisão financeira.

O aplicativo é gratuito para as famílias e se sustenta por parcerias com prefeituras e CRAS, cooperativas de crédito, ONGs e distribuidoras de energia, instituições que já atendem esse público e se beneficiam de uma população menos endividada e mais consciente do próprio consumo. A proposta contribui para os ODS 10 (Redução das desigualdades), 4 (Educação de qualidade) e 13 (Ação contra a mudança global do clima). O impacto será medido principalmente pela proporção de famílias que deixam a faixa Vermelha do Termômetro em até três meses de uso.

Esta solução foi desenvolvida para a Global Solution 2026/2 do curso de Análise e Desenvolvimento de Sistemas da FIAP. Ela é composta por entregas integradas: uma API principal em Java, uma API de conteúdo educativo em .NET, o aplicativo em Flutter, a camada de processamento em PL/SQL, o medidor IoT e a infraestrutura em nuvem no Azure.

## Relação com o tema CitizenTech

CitizenTech é tecnologia a serviço do cidadão. Aqui, o cidadão atendido é a família de baixa renda endividada, e o serviço é a educação financeira gratuita, em linguagem simples e sem exigir cadastro. A distribuição acontece por instituições que já atendem esse público, como prefeituras, CRAS, cooperativas de crédito e ONGs. Esta API é a parte da solução que guarda e entrega esse conteúdo educativo: as trilhas e os desafios que a família joga na área aberta do aplicativo.

## Sobre este repositório

Este repositório contém a **API de conteúdo educativo** do Grana Amiga, desenvolvida em ASP.NET Core 8 com Entity Framework Core e banco de dados Oracle. Ela cadastra e disponibiliza as trilhas de aprendizagem, como "Fugindo dos Juros" e "O Desafio do Supermercado", e os desafios de múltipla escolha que compõem cada uma delas, modelados em uma relação 1:N. Esses dados alimentam os jogos da área aberta do aplicativo: cada desafio traz a pergunta, as alternativas, a resposta correta e a pontuação que a família acumula ao jogar. Manter o conteúdo educativo em uma API própria permite publicar, revisar ou ter trilhas patrocinadas por parceiros sem alterar o aplicativo nem a API de dados financeiros.

## Relação com as demais entregas

Esta API fornece o conteúdo dos jogos da área aberta do aplicativo Flutter, exibidos na tela Jogo/Simulador. Nesta fase, o aplicativo usa dados mockados com os mesmos campos de Trilha e Desafio expostos aqui.

A API Java é a API principal da solução: cuida de usuários, transações, dívidas e energia, e é ela que roda em containers na entrega de DevOps. As duas APIs aparecem na camada de Aplicação do diagrama ArchiMate.

O banco da entrega de PL/SQL usa outras tabelas, em outro schema, e não se confunde com as tabelas `TRILHA` e `DESAFIO` deste projeto.

## Tecnologias

- ASP.NET Core 8 (Web API com controllers, `net8.0`)
- Entity Framework Core 8.0.33 (`Microsoft.EntityFrameworkCore` e `Microsoft.EntityFrameworkCore.Design`)
- Oracle.EntityFrameworkCore 8.23.26400
- Oracle Database da FIAP
- Migrations do EF Core para criar as tabelas e a carga inicial
- Swagger (Swashbuckle.AspNetCore 6.6.2) e Scalar (Scalar.AspNetCore 2.19.0) para a documentação interativa

## Modelo de dados

```mermaid
erDiagram
    TRILHA ||--o{ DESAFIO : possui

    TRILHA {
        NUMBER(10) ID PK "identity"
        VARCHAR2(100) TITULO "obrigatório, 3 a 100 caracteres"
        VARCHAR2(500) DESCRICAO "opcional"
        VARCHAR2(20) TEMA "JUROS, CONSUMO ou POUPANCA"
        NUMBER(1) NIVEL "1 a 3"
        TIMESTAMP DT_CRIACAO "definida na criação"
    }

    DESAFIO {
        NUMBER(10) ID PK "identity"
        NUMBER(10) TRILHA_ID FK "referencia TRILHA.ID"
        VARCHAR2(500) PERGUNTA "obrigatório, mínimo 10 caracteres"
        VARCHAR2(200) ALTERNATIVA_A "obrigatório"
        VARCHAR2(200) ALTERNATIVA_B "obrigatório"
        VARCHAR2(200) ALTERNATIVA_C "obrigatório"
        VARCHAR2(200) ALTERNATIVA_D "obrigatório"
        CHAR(1) RESPOSTA_CORRETA "A, B, C ou D"
        NUMBER(3) PONTOS "1 a 100"
    }
```

Uma trilha possui vários desafios e cada desafio pertence a exatamente uma trilha (1:N), pela chave estrangeira `DESAFIO.TRILHA_ID`. A FK foi criada com `ON DELETE CASCADE`, então remover uma trilha remove também os desafios dela.

## Endpoints

| Verbo | Rota | Descrição | Sucesso | Erros |
|---|---|---|---|---|
| GET | `/api/trilhas` | Lista todas as trilhas, com a quantidade de desafios de cada uma | 200 | — |
| GET | `/api/trilhas/{id}` | Busca uma trilha pelo id | 200 | 404 |
| GET | `/api/trilhas/{id}/desafios` | Lista os desafios de uma trilha | 200 | 404 (trilha inexistente) |
| POST | `/api/trilhas` | Cria uma trilha | 201 | 400 |
| PUT | `/api/trilhas/{id}` | Atualiza uma trilha | 204 | 400, 404 |
| DELETE | `/api/trilhas/{id}` | Remove uma trilha e os desafios dela | 204 | 404 |
| GET | `/api/desafios` | Lista os desafios; aceita o filtro opcional `?trilhaId=` | 200 | — |
| GET | `/api/desafios/{id}` | Busca um desafio pelo id | 200 | 404 |
| POST | `/api/desafios` | Cria um desafio em uma trilha existente | 201 | 400 (dados inválidos ou trilha inexistente) |
| PUT | `/api/desafios/{id}` | Atualiza um desafio | 204 | 400 (dados inválidos ou trilha inexistente), 404 |
| DELETE | `/api/desafios/{id}` | Remove um desafio | 204 | 404 |

Listas vazias respondem 200 com `[]`. Dados inválidos (400) e ids inexistentes (404) respondem com corpo JSON no formato `{ "erro": "mensagem" }`.

A validação acontece em duas camadas: Data Annotations nos DTOs de entrada e regras nas próprias entidades, que lançam `DomainException`. Um tratador global de exceções (`Exceptions/GlobalExceptionHandler.cs`) converte essas exceções em 400 e qualquer erro inesperado em 500 com `{ "erro": "Erro interno." }`, por isso os controllers não repetem blocos `try-catch`.

## Como executar

### Pré-requisitos

- .NET 8 SDK, ou um SDK mais novo com o runtime ASP.NET Core 8 instalado.
- Ferramenta `dotnet-ef` 8.x:

  ```bash
  dotnet tool install --global dotnet-ef --version 8.0.*
  ```

  Se a ferramenta já estiver instalada em outra versão, use `dotnet tool update --global dotnet-ef --version 8.0.*`.

- Acesso ao Oracle da FIAP (usuário e senha).

### Passo a passo

1. Clone o repositório e entre na pasta:

   ```bash
   git clone https://github.com/MaThPMJ/trilhas-api.git
   cd trilhas-api
   ```

2. Configure a connection string. A senha fica fora do repositório: o `appsettings.json` traz apenas um valor de exemplo, e o valor real é guardado pelo `user-secrets` na sua máquina. O user-secrets é lido quando a API roda em ambiente Development, caso do `dotnet run --launch-profile http`; fora dele, vale a variável de ambiente.

   ```bash
   dotnet user-secrets set "ConnectionStrings:OracleConnection" "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
   ```

   Como alternativa, defina a variável de ambiente `ConnectionStrings__OracleConnection` com o mesmo valor:

   ```powershell
   $env:ConnectionStrings__OracleConnection = "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
   ```

   ```bash
   export ConnectionStrings__OracleConnection="User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
   ```

3. Aplique as migrations. Este comando cria as tabelas `TRILHA` e `DESAFIO` e insere a carga inicial (3 trilhas e 12 desafios):

   ```bash
   dotnet ef database update
   ```

4. Suba a API:

   ```bash
   dotnet run --launch-profile http
   ```

5. Abra a documentação interativa no Scalar: <http://localhost:5099/scalar/v1>. O Swagger UI também fica disponível em <http://localhost:5099/swagger>.

## Como testar

### Pelo Scalar

Com a API no ar, abra <http://localhost:5099/scalar/v1>, escolha um endpoint, preencha os dados e envie a requisição pela própria página.

### Pelo arquivo TrilhasApi.http

O arquivo `TrilhasApi.http`, na raiz, traz requisições prontas para todos os endpoints, incluindo os casos de 400 e 404. Ele pode ser executado pelo Visual Studio ou pelo VS Code (extensão REST Client). Execute as requisições na ordem do arquivo: as que usam um id dependem da resposta de `criarTrilha` e `criarDesafio`.

### Exemplos por rota — Trilhas

Nos exemplos abaixo, os valores de `id` e `dtCriacao` são ilustrativos: o id é gerado pelo banco e a data é a do momento da criação.

**`GET /api/trilhas`** → 200, com a lista de trilhas (ou `[]` se não houver nenhuma).

**`POST /api/trilhas`** → 201, com o cabeçalho `Location` apontando para a trilha criada.

```json
{
  "titulo": "Fugindo dos Juros",
  "descricao": "Entenda como os juros do cartão fazem a dívida crescer.",
  "tema": "JUROS",
  "nivel": 1
}
```

`tema` aceita `JUROS`, `CONSUMO` ou `POUPANCA`; `nivel` vai de 1 a 3; `descricao` é opcional.

Resposta:

```json
{
  "id": 4,
  "titulo": "Fugindo dos Juros",
  "descricao": "Entenda como os juros do cartão fazem a dívida crescer.",
  "tema": "JUROS",
  "nivel": 1,
  "dtCriacao": "2026-11-14T10:30:00",
  "quantidadeDesafios": 0
}
```

**`GET /api/trilhas/4`** → 200, com o mesmo formato da resposta acima.

**`GET /api/trilhas/4/desafios`** → 200, com a lista de desafios da trilha (ou `[]`).

**`PUT /api/trilhas/4`** → 204, sem corpo na resposta.

```json
{
  "titulo": "Fugindo dos Juros do Cartão",
  "descricao": null,
  "tema": "JUROS",
  "nivel": 2
}
```

**`DELETE /api/trilhas/4`** → 204, sem corpo. Os desafios da trilha são removidos junto.

### Exemplos por rota — Desafios

**`GET /api/desafios`** → 200, com a lista de desafios (ou `[]`).

**`GET /api/desafios?trilhaId=1`** → 200, apenas com os desafios da trilha 1.

**`POST /api/desafios`** → 201, com o cabeçalho `Location` apontando para o desafio criado. O `trilhaId` precisa ser de uma trilha existente.

```json
{
  "trilhaId": 1,
  "pergunta": "O que acontece quando você paga só o valor mínimo da fatura?",
  "alternativaA": "A dívida some no mês seguinte",
  "alternativaB": "O resto da fatura vira dívida com juros altos",
  "alternativaC": "O banco dá um desconto na próxima fatura",
  "alternativaD": "O limite do cartão aumenta sozinho",
  "respostaCorreta": "B",
  "pontos": 10
}
```

As quatro alternativas devem ser diferentes entre si; `respostaCorreta` aceita `A`, `B`, `C` ou `D`; `pontos` vai de 1 a 100.

Resposta:

```json
{
  "id": 13,
  "trilhaId": 1,
  "tituloTrilha": "Fugindo dos Juros",
  "pergunta": "O que acontece quando você paga só o valor mínimo da fatura?",
  "alternativaA": "A dívida some no mês seguinte",
  "alternativaB": "O resto da fatura vira dívida com juros altos",
  "alternativaC": "O banco dá um desconto na próxima fatura",
  "alternativaD": "O limite do cartão aumenta sozinho",
  "respostaCorreta": "B",
  "pontos": 10
}
```

**`GET /api/desafios/13`** → 200, com o mesmo formato da resposta acima.

**`PUT /api/desafios/13`** → 204, sem corpo na resposta.

```json
{
  "trilhaId": 1,
  "pergunta": "O que acontece quando você paga só o valor mínimo da fatura do cartão?",
  "alternativaA": "A dívida some no mês seguinte",
  "alternativaB": "O resto da fatura vira dívida com juros altos",
  "alternativaC": "O banco dá um desconto na próxima fatura",
  "alternativaD": "O limite do cartão aumenta sozinho",
  "respostaCorreta": "B",
  "pontos": 20
}
```

**`DELETE /api/desafios/13`** → 204, sem corpo.

### Exemplos de erro

**400 — dados inválidos.** `POST /api/trilhas` com título de 2 caracteres:

```json
{
  "titulo": "ab",
  "descricao": "Título com menos de 3 caracteres.",
  "tema": "JUROS",
  "nivel": 1
}
```

Resposta:

```json
{
  "erro": "Dados inválidos.",
  "detalhes": {
    "Titulo": ["O título deve ter entre 3 e 100 caracteres."]
  }
}
```

**400 — regra da entidade.** `POST /api/desafios` com duas alternativas iguais:

```json
{
  "erro": "As alternativas devem ser distintas entre si."
}
```

**400 — trilha inexistente.** `POST` ou `PUT` em `/api/desafios` com um `trilhaId` que não existe:

```json
{
  "erro": "Trilha informada não existe."
}
```

**404 — id inexistente.** `GET`, `PUT` ou `DELETE` em `/api/trilhas/99999`, e também `GET /api/trilhas/99999/desafios`:

```json
{
  "erro": "Trilha 99999 não encontrada."
}
```

**404 — id inexistente.** `GET`, `PUT` ou `DELETE` em `/api/desafios/99999`:

```json
{
  "erro": "Desafio 99999 não encontrado."
}
```

### Conferindo os dados no Oracle

```sql
SELECT t.ID,
       t.TITULO,
       t.TEMA,
       t.NIVEL,
       d.ID AS DESAFIO_ID,
       d.PERGUNTA,
       d.RESPOSTA_CORRETA,
       d.PONTOS
  FROM TRILHA t
  JOIN DESAFIO d ON d.TRILHA_ID = t.ID
 ORDER BY t.ID, d.ID;
```

## Vídeo

[LINK DO VÍDEO]
