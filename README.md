# 📚 Livro - Sistema de Gerenciamento de Biblioteca

Sistema completo de gerenciamento de livros desenvolvido com **.NET 10** (API) e **Angular 19** (Frontend), utilizando **Clean Architecture** e boas práticas de desenvolvimento.
## Opção 1: Docker (RECOMENDADO) - 1 comando ⚡

```bash
docker-compose up --build
```

**Pronto!** Aguarde ~2 minutos e acesse:
- 🌐 **Aplicação Web**: http://localhost:4200
- 🔧 **API + Swagger**: http://localhost:5000/swagger
- 📊 **Banco SQLite**: Criado automaticamente em `./data/livro.db`

### Parar tudo
```bash
docker-compose down
```

---

## Opção 2: Desenvolvimento Local (sem Docker)

### Pré-requisitos
- .NET 10 SDK
- Node.js 20+

### Passo 1: Rodar Backend

```bash
cd livro_api/src/Livro.Presentation.Api
dotnet run
```

✅ API rodando em: http://localhost:5214  
✅ Swagger: http://localhost:5214/swagger

### Passo 2: Rodar Frontend (em outro terminal)

```bash
cd livro_presentation_angular/livro-app
npm install
npm start
```

✅ Web rodando em: http://localhost:4200

---

## 🎯 O que o sistema faz?

1. **CRUD de Autores** - Gerenciar autores de livros
2. **CRUD de Assuntos** - Categorias/temas dos livros
3. **CRUD de Livros** - Cadastro completo com:
   - Múltiplos autores
   - Múltiplos assuntos
   - Valores por tipo de compra (Balcão, Internet, Evento)
4. **Relatório** - Visualização agrupada por autor com:
   - ✅ Consulta em VIEW do banco
   - 📄 Exportação PDF
   - 🖨️ Impressão

---


## 📋 Checklist de Validação

### Backend ✅
- [ ] API responde em http://localhost:5000
- [ ] Swagger funciona em http://localhost:5000/swagger
- [ ] GET `/api/autor` retorna array (pode estar vazio)
- [ ] GET `/api/tipo-compra` retorna 4 tipos (seed automático)
- [ ] GET `/api/relatorio/livro` retorna dados da VIEW
- [ ] Banco SQLite criado em `data/livro.db`

### Frontend ✅
- [ ] App carrega em http://localhost:4200

## 📦 Estrutura do Projeto

```
livro/
├── livro_api/              # Backend .NET 10 - Clean Architecture
│   ├── src/
│   │   ├── Livro.Domain/           # Entidades, Ports, Regras
│   │   ├── Livro.Application/      # Use Cases
│   │   ├── Livro.Infra.EfCore/     # EF Core, Adapters, Migrations
│   │   ├── Livro.Presentation.Api/ # Controllers REST
│   │   └── Livro.Presentation.Hosting/ # DI, Startup
│   ├── test/               # testes unitários
│   └── Dockerfile
│
├── livro_presentation_angular/  # Frontend Angular 19
│   ├── livro-app/
│   │   └── src/app/
│   │       ├── core/       # Models, Services
│   │       ├── features/   # Autores, Assuntos, Livros, Relatório
│   │       └── shared/     # Componentes compartilhados
│   └── Dockerfile
│
├── docker-compose.yml      # Orquestração (API + Web + Network)
├── .env                    # Variáveis de ambiente
├── START.md               # ← VOCÊ ESTÁ AQUI
└── README.md              # Documentação completa
```

---

## 🎯 Diferenciais do Projeto

✅ **Clean Architecture** (Domain → Application → Infrastructure → Presentation)  
✅ **Ports & Adapters** (Hexagonal Architecture)  
✅ **ULID** para IDs (sortable, performance melhor que GUID)  
✅ **SQLite** (zero configuração, portátil)  
✅ **Migrations** automáticas (EF Core)  
✅ **Seed** automático (TipoCompra)  
✅ **VIEW no banco** (relatório obrigatório)  
✅ **Testes unitários** (54 testes com xUnit + FluentAssertions)  
✅ **Swagger/OpenAPI** (documentação interativa)  
✅ **Docker** ready (1 comando para subir tudo)  
✅ **CORS** configurado  
✅ **Validações** client-side e server-side  
✅ **Bootstrap 5** (interface responsiva)  
✅ **PDF Export** (jsPDF + jsPDF-AutoTable)  
✅ **Print** support (media query @print)  


