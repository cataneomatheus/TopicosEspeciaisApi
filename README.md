# TopicosEspeciaisApi

Backend do projeto **Cartola FC** — uma Web API em .NET que gerencia jogadores de um time de futebol no estilo Cartola, com formacao 4-4-2.

Este projeto foi criado para a disciplina de Topicos Especiais, com foco em ensinar **deploy no Render via GitHub**.

---

## O que este projeto faz?

E uma API REST que permite montar um time de futebol completo. Voce pode:

- Listar todos os jogadores do time
- Buscar um jogador especifico pelo ID
- Adicionar um novo jogador em uma posicao do campo
- Editar as informacoes de um jogador existente
- Remover um jogador do time

Os dados ficam **em memoria** (nao usa banco de dados). Isso significa que toda vez que a aplicacao reinicia, o time volta ao estado inicial — a selecao brasileira do Penta 2002.

---

## Estrutura do projeto

```
TopicosEspeciaisApi/
├── Program.cs                  # Ponto de entrada — configura tudo e define os endpoints
├── TopicosEspeciaisApi.csproj  # Arquivo de projeto .NET (dependencias, target framework)
├── Models/
│   └── Player.cs               # Modelo de dados do jogador
├── Services/
│   └── PlayerService.cs        # Logica de CRUD in-memory + dados iniciais (seed)
├── Dockerfile                  # Instrucoes para criar a imagem Docker
├── .dockerignore               # Arquivos ignorados pelo Docker
├── render.yaml                 # Blueprint de deploy automatico no Render
├── appsettings.json            # Configuracoes do ASP.NET (padrao)
└── appsettings.Development.json
```

---

## Como rodar localmente

### Pre-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) instalado (versao 8.0 ou superior)

### Passo a passo

```bash
# 1. Clone o repositorio
git clone https://github.com/SEU_USUARIO/TopicosEspeciaisApi.git
cd TopicosEspeciaisApi

# 2. Restaure as dependencias e rode
dotnet run
```

Pronto! A API vai subir em **http://localhost:5000**.

### Acessando o Swagger

Abra no navegador:

```
http://localhost:5000/swagger
```

O Swagger e uma interface visual que lista todos os endpoints da API. Voce pode testar cada um diretamente pelo navegador — clicar em "Try it out", preencher os campos e ver a resposta. E a melhor forma de explorar a API sem precisar de Postman ou outra ferramenta.

---

## Endpoints da API

| Metodo   | Rota                 | O que faz                  | Retorno           |
|----------|----------------------|----------------------------|--------------------|
| `GET`    | `/api/players`       | Lista todos os jogadores   | `200` com array    |
| `GET`    | `/api/players/{id}`  | Busca jogador por ID       | `200` ou `404`     |
| `POST`   | `/api/players`       | Cria um novo jogador       | `201` com jogador  |
| `PUT`    | `/api/players/{id}`  | Atualiza um jogador        | `200` ou `404`     |
| `DELETE` | `/api/players/{id}`  | Remove um jogador          | `204` ou `404`     |

### Exemplo de body para POST/PUT

```json
{
  "name": "Neymar",
  "age": 30,
  "position": "FWD",
  "photoUrl": "https://exemplo.com/neymar.jpg",
  "slotIndex": 9
}
```

### Posicoes validas

| Valor   | Posicao            |
|---------|--------------------|
| `GK`    | Goleiro            |
| `DEF`   | Zagueiro / Lateral |
| `MID`   | Meio-campo         |
| `FWD`   | Atacante           |
| `COACH` | Tecnico            |

### Slots do campo (formacao 4-4-2)

```
Slot 0:     GK      (Goleiro)
Slot 1-4:   DEF     (Lateral esq, Zagueiro, Zagueiro, Lateral dir)
Slot 5-8:   MID     (Meio esq, Meia centro, Meia centro, Meio dir)
Slot 9-10:  FWD     (Atacante esquerdo, Atacante direito)
Slot 11:    COACH   (Tecnico — fora do campo)
```

---

## Entendendo as configuracoes

### Variaveis de ambiente

A API usa **variaveis de ambiente** para se configurar. Isso e fundamental para funcionar tanto localmente quanto no Render (ou qualquer outro servidor).

| Variavel       | Para que serve                                      | Valor padrao |
|----------------|-----------------------------------------------------|--------------|
| `PORT`         | Porta em que a API vai escutar                      | `5000`       |
| `FRONTEND_URL` | URL do frontend permitida no CORS                   | `*` (todas)  |

#### PORT — Por que existe?

Quando voce roda `dotnet run` no seu computador, a API sobe na porta 5000. Mas no Render, o servidor decide em qual porta a aplicacao deve rodar e informa via variavel `PORT`. O Render **exige** que sua aplicacao escute nessa porta — se nao escutar, o deploy falha.

No codigo (`Program.cs`):
```csharp
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
```

O `0.0.0.0` significa "escute em todas as interfaces de rede". Isso e necessario dentro de containers Docker, porque `localhost` so funciona internamente.

#### FRONTEND_URL — O que e CORS?

CORS (Cross-Origin Resource Sharing) e uma protecao do navegador. Se o frontend esta em `https://meusite.com` e tenta acessar uma API em `https://api.meusite.com`, o navegador bloqueia a requisicao por padrao.

Para liberar, a API precisa dizer explicitamente: "eu aceito requisicoes vindas desse endereco". E isso que o `FRONTEND_URL` faz.

- Em **desenvolvimento**: o valor padrao e `*` (aceita de qualquer lugar) — mais facil para testar
- Em **producao**: voce deve colocar a URL exata do frontend, ex: `https://topicos-especiais-front.onrender.com`

No codigo:
```csharp
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "*";
```

---

## Dados iniciais (Seed)

Quando a API inicia, o `PlayerService` ja carrega um time completo — a selecao brasileira do **Pentacampeonato 2002**:

| Slot | Posicao | Jogador            |
|------|---------|--------------------|
| 0    | GK      | Marcos             |
| 1    | DEF     | Cafu               |
| 2    | DEF     | Lucio              |
| 3    | DEF     | Roque Junior       |
| 4    | DEF     | Roberto Carlos     |
| 5    | MID     | Gilberto Silva     |
| 6    | MID     | Kleberson          |
| 7    | MID     | Ronaldinho         |
| 8    | MID     | Rivaldo            |
| 9    | FWD     | Ronaldo            |
| 10   | FWD     | Ronaldinho Gaucho  |
| 11   | COACH   | Felipao            |

Como os dados sao in-memory, se voce remover todos os jogadores e reiniciar a API, o time volta ao normal.

---

## Docker — Para que serve o Dockerfile?

O **Dockerfile** e um arquivo de instrucoes que ensina o Docker a criar uma "imagem" da sua aplicacao. Essa imagem e como uma foto do seu projeto ja compilado e pronto para rodar — com tudo que precisa, sem depender do que esta instalado na maquina.

### Por que isso importa?

Sem Docker, para rodar a API voce precisaria:
1. Instalar o .NET SDK na maquina
2. Clonar o repositorio
3. Rodar `dotnet restore` e `dotnet run`

Com Docker, voce so precisa:
1. Ter o Docker instalado
2. Rodar **um comando**

E mais: o Render usa o Dockerfile para fazer o deploy. Quando voce conecta o repositorio, o Render le o Dockerfile, cria a imagem e roda automaticamente.

### Entendendo o Dockerfile (Multi-stage Build)

```dockerfile
# ── Estagio 1: BUILD ──────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish
```

**O que acontece aqui:**
1. Usa a imagem do **SDK** do .NET (pesada, ~700MB — tem compilador, ferramentas, tudo)
2. Copia so o `.csproj` primeiro e roda `dotnet restore` — isso cria um cache das dependencias. Se voce mudar so o codigo (nao o .csproj), o Docker reusa esse cache e o build fica mais rapido
3. Copia o resto do codigo e compila em modo Release

```dockerfile
# ── Estagio 2: RUNTIME ────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

COPY --from=build /app/publish .

ENV PORT=10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "TopicosEspeciaisApi.dll"]
```

**O que acontece aqui:**
1. Troca para a imagem do **ASP.NET Runtime** (leve, ~200MB — so roda, nao compila)
2. Copia apenas os arquivos compilados do estagio anterior
3. Define a porta 10000 (padrao do Render) e inicia a aplicacao

**Por que dois estagios?** Porque a imagem final nao precisa do SDK. Isso deixa a imagem menor (mais rapida para deploy) e mais segura (menos ferramentas expostas).

### Testando o Docker localmente

```bash
# Criar a imagem
docker build -t topicos-api .

# Rodar o container
docker run -p 10000:10000 topicos-api

# Testar
curl http://localhost:10000/api/players
```

### O que e o .dockerignore?

Funciona igual ao `.gitignore`, mas para o Docker. Ele diz ao Docker quais arquivos/pastas **nao** devem ser copiados para dentro da imagem. Isso evita copiar coisas desnecessarias como `bin/`, `obj/` e `.git/`, deixando o build mais rapido.

---

## Deploy no Render

### O que e o render.yaml?

O `render.yaml` e um **Blueprint** — um arquivo de configuracao que diz ao Render exatamente como fazer o deploy do projeto. Quando voce conecta o repositorio no Render e ele encontra esse arquivo, as configuracoes sao aplicadas automaticamente.

```yaml
services:
  - type: web             # Tipo: Web Service (fica online recebendo requisicoes)
    name: topicos-especiais-api   # Nome do servico no painel do Render
    runtime: docker       # Usa Docker (vai ler o Dockerfile)
    branch: main          # Branch do Git que dispara o deploy
    envVars:
      - key: PORT
        value: "10000"    # Porta fixa que o Render espera
      - key: FRONTEND_URL
        sync: false       # Voce configura manualmente no painel do Render
```

### Passo a passo para deploy

1. **Suba o codigo para o GitHub** — crie um repositorio e faca push da branch `main`
2. **Acesse o [Render](https://render.com)** e crie uma conta (pode ser com GitHub)
3. **New > Web Service** — conecte o repositorio `TopicosEspeciaisApi`
4. O Render vai detectar o `Dockerfile` automaticamente
5. Em **Environment Variables**, configure:
   - `PORT` = `10000`
   - `FRONTEND_URL` = URL do frontend (depois que fizer deploy dele)
6. Clique em **Create Web Service**
7. Aguarde o build e deploy (primeira vez demora ~2-3 minutos)

### Deploy automatico

Toda vez que voce fizer `git push` na branch `main`, o Render automaticamente:
1. Detecta a mudanca
2. Reconstroi a imagem Docker
3. Substitui o container antigo pelo novo

Ou seja: **fez push, ja ta no ar**. Zero configuracao adicional.

---

## Arquitetura simplificada

```
Requisicao HTTP
      |
      v
  Program.cs (Minimal API)        ← Define as rotas e configura a app
      |
      v
  PlayerService (Singleton)       ← Logica de negocio + dados em memoria
      |
      v
  List<Player>                    ← "Banco de dados" in-memory
```

- **Program.cs**: Ponto de entrada. Configura Swagger, CORS, porta, e define os 5 endpoints da API usando Minimal API (sem Controllers).
- **PlayerService**: Registrado como Singleton (uma unica instancia para toda a aplicacao). Contem a lista de jogadores e os metodos de CRUD.
- **Player**: Modelo simples com Id, Name, Age, Position, PhotoUrl e SlotIndex.

---

## Duvidas comuns

**P: Por que nao usar banco de dados?**
R: Porque o foco e ensinar deploy, nao persistencia. Manter in-memory simplifica o projeto e evita configuracao de banco.

**P: Os dados somem quando reinicia?**
R: Sim. E proposital. Os dados seed (selecao 2002) voltam toda vez que a aplicacao reinicia.

**P: Posso adicionar banco depois?**
R: Sim! Basta trocar a `List<Player>` por um contexto do Entity Framework (DbContext) e apontar para um banco PostgreSQL (o Render oferece PostgreSQL gratis no plano free).

**P: O que e Minimal API?**
R: E o estilo simplificado de definir endpoints no .NET, sem precisar criar classes Controller. Tudo fica no `Program.cs`. Ideal para APIs pequenas e projetos didaticos.
