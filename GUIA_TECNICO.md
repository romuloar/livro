# 📘 Guia Técnico Completo - Sistema de Livros

## 🎯 Sumário Executivo

Este documento serve como guia de estudo completo para apresentação e defesa técnica do projeto. Aqui você encontrará explicações detalhadas sobre todas as decisões arquiteturais, padrões utilizados e justificativas técnicas.

---

## 🔷 PARTE 1: FRONTEND ANGULAR

### 1.1 Arquitetura Angular - Standalone Components

#### Por que NÃO usamos NgModules?

O projeto foi desenvolvido com **Standalone Components** (padrão desde Angular 14+), que é a abordagem moderna e recomendada pela equipe Angular. 

**Justificativa técnica:**
```typescript
// ❌ Abordagem antiga (NgModules)
@NgModule({
  declarations: [LivroList, LivroForm],
  imports: [CommonModule, FormsModule],
  exports: [LivroList]
})
export class LivroModule { }

// ✅ Abordagem moderna (Standalone)
@Component({
  selector: 'app-livro-list',
  standalone: true, // Não precisa de NgModule
  imports: [CommonModule, RouterModule], // Importa diretamente
  templateUrl: './livro-list.html',
  styleUrl: './livro-list.css'
})
export class LivroList { }
```

**Vantagens:**
- **Tree-shaking melhor** → Bundle menor em produção
- **Menos boilerplate** → Não precisa criar módulos para cada feature
- **Lazy loading simplificado** → Componentes são carregados sob demanda
- **Migração gradual** → Pode misturar standalone com NgModules (se necessário)

---

### 1.2 Estrutura de Pastas - `features` vs `views` vs `pages`

#### Por que escolhi `features`?

```
src/app/
├── core/           # Serviços singleton, models, guards
├── shared/         # Componentes reutilizáveis (navbar, toast)
├── features/       # ← ESCOLHA: Módulos de funcionalidade
│   ├── autor/
│   ├── assunto/
│   └── livro/
```

**Justificativa técnica:**

A nomenclatura `features` é baseada em **Feature-Sliced Design** e **Domain-Driven Design (DDD)**:

1. **`pages/` ou `views/`** → Sugere apenas UI/apresentação
2. **`features/`** → Sugere **funcionalidade completa** (componentes + lógica + estado)

**Cada feature é auto-contida:**
```
features/livro/
├── livro-list/          # Listagem
│   ├── livro-list.ts
│   ├── livro-list.html
│   └── livro-list.css
├── livro-form/          # Formulário
│   ├── livro-form.ts
│   ├── livro-form.html
│   └── livro-form.css
```

**Isso facilita:**
- **Escalabilidade** → Adicionar nova feature não impacta outras
- **Testes** → Cada feature tem seus próprios testes
- **Code splitting** → Lazy load por feature
- **Trabalho em equipe** → Times diferentes podem trabalhar em features diferentes

**Padrão da indústria:**
- Google (Angular docs): Usa `features`
- Nx Monorepo: Usa `features` ou `libs`
- Empresas como Microsoft, SAP, Oracle: Usam variações de `features` ou `modules`

---

### 1.3 Gerenciamento de Estado - Signals (Angular 16+)

#### "No Vue usamos Pinia, aqui estamos usando o quê?"

**Resposta:** Angular **Signals** (reatividade nativa desde Angular 16)

#### O que são Signals?

Signals são primitivas de **reatividade fina** (fine-grained reactivity), similar a:
- **Vue 3:** `ref()` e `reactive()`
- **SolidJS:** `createSignal()`
- **Svelte:** `$:`

**Exemplo prático no projeto:**

```typescript
// livro-list.ts
import { signal } from '@angular/core';

export class LivroList implements OnInit {
  // ✅ Estado reativo com Signals
  livros = signal<Livro[]>([]);      // Estado inicial: array vazio
  loading = signal(true);             // Estado de carregamento
  error = signal<string | null>(null); // Mensagens de erro

  ngOnInit(): void {
    this.loadLivros();
  }

  loadLivros(): void {
    this.loading.set(true); // Atualiza estado
    this.livroService.getAll().subscribe({
      next: (data) => {
        this.livros.set(data);    // Atualiza array de livros
        this.loading.set(false);  // Desativa loading
      },
      error: (err) => {
        this.error.set('Erro ao carregar');
        this.loading.set(false);
      }
    });
  }
}
```

**No template (HTML):**
```html
<!-- Acessa o valor com () -->
<div *ngIf="loading()">Carregando...</div>
<div *ngIf="error()">{{ error() }}</div>

<table *ngIf="!loading()">
  <tr *ngFor="let livro of livros()">
    <td>{{ livro.titulo }}</td>
  </tr>
</table>
```

#### Signals vs Outras Soluções de Estado

| Solução | Quando usar |
|---------|-------------|
| **Signals** | Estado local de componente (99% dos casos) |
| **Services com BehaviorSubject** | Estado compartilhado entre componentes (ex: `ToastService`) |
| **NgRx** | Aplicações complexas com muitas interações estado global |
| **Akita/Elf** | Alternativas ao NgRx (menos verboso) |

**Por que NÃO usamos NgRx neste projeto?**

NgRx adiciona **complexidade desnecessária** para um CRUD simples:
- ❌ Boilerplate: Actions, Reducers, Effects, Selectors
- ❌ Curva de aprendizado alta
- ✅ Signals resolvem 90% dos casos com código mais simples

**Quando NgRx seria necessário:**
- Aplicações com estado global complexo (ex: e-commerce com carrinho, usuário, produtos, checkout)
- Time travel debugging
- Undo/Redo
- Persistência de estado (LocalStorage, IndexedDB)

---

### 1.4 Reactive Forms - Por que não Template-driven?

**Escolha:** Reactive Forms (FormBuilder, FormGroup, FormControl)

```typescript
// livro-form.ts
export class LivroForm implements OnInit {
  livroForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.livroForm = this.fb.group({
      titulo: ['', [Validators.required, Validators.maxLength(40)]],
      editora: ['', [Validators.maxLength(40)]],
      edicao: [null, [Validators.required]],
      anoPublicacao: ['', [Validators.required, Validators.maxLength(4)]],
      autores: [[]],      // Array de IDs
      assuntos: [[]],     // Array de IDs
      valores: this.fb.array([]) // FormArray dinâmico
    });
  }
}
```

**Vantagens sobre Template-driven:**

| Reactive Forms | Template-driven |
|----------------|-----------------|
| ✅ Validação programática | ❌ Validação no template |
| ✅ Testes unitários fáceis | ❌ Difícil testar |
| ✅ Validação assíncrona | ❌ Limitado |
| ✅ FormArray (campos dinâmicos) | ❌ Complexo |
| ✅ Tipagem forte (TypeScript) | ❌ Sem tipagem |

**Exemplo de FormArray (valores dinâmicos):**

```typescript
get valores(): FormArray {
  return this.livroForm.get('valores') as FormArray;
}

addValor(tipoCompraId: string, valor: number): void {
  const valorForm = this.fb.group({
    tipoCompraId: [tipoCompraId, Validators.required],
    valor: [valor, [Validators.required, Validators.min(0)]]
  });
  this.valores.push(valorForm);
}

removeValor(index: number): void {
  this.valores.removeAt(index);
}
```

**No template:**
```html
<div formArrayName="valores">
  <div *ngFor="let valor of valores.controls; let i = index" [formGroupName]="i">
    <input formControlName="tipoCompraId" />
    <input formControlName="valor" type="number" />
    <button (click)="removeValor(i)">Remover</button>
  </div>
</div>
```

---

### 1.5 Services e Injeção de Dependência

#### Padrão Repository no Frontend

```typescript
// autor.service.ts
@Injectable({
  providedIn: 'root' // Singleton em toda aplicação
})
export class AutorService {
  private apiUrl = `${environment.apiUrl}/autor`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Autor[]> {
    return this.http.get<ApiResult<Autor[]>>(this.apiUrl).pipe(
      map(result => result?.resultData ?? []),
      catchError((error) => {
        if (error.status === 404) return of([]);
        return throwError(() => error);
      })
    );
  }
}
```

**Por que `providedIn: 'root'`?**

- ✅ **Singleton** → Uma única instância em toda app
- ✅ **Tree-shakeable** → Se não for usado, não vai pro bundle
- ❌ Alternativa antiga: Declarar em `providers: []` do NgModule

---

### 1.6 RxJS - Programação Reativa

**Operadores usados no projeto:**

```typescript
// map: Transforma dados
getAll(): Observable<Autor[]> {
  return this.http.get<ApiResult<Autor[]>>(this.apiUrl).pipe(
    map(result => result.resultData) // Extrai dados do wrapper
  );
}

// catchError: Trata erros
getAll(): Observable<Autor[]> {
  return this.http.get<ApiResult<Autor[]>>(this.apiUrl).pipe(
    catchError((error) => {
      if (error.status === 404) return of([]); // Retorna array vazio
      return throwError(() => error); // Re-lança erro
    })
  );
}

// of: Cria Observable a partir de valor
return of([]);
```

**Por que RxJS e não Promises?**

| RxJS (Observables) | Promises |
|--------------------|----------|
| ✅ Cancelável | ❌ Não cancelável |
| ✅ Múltiplos valores ao longo do tempo | ❌ Valor único |
| ✅ Operadores (map, filter, debounce) | ❌ Apenas .then() |
| ✅ Lazy (não executa até subscribe) | ❌ Eager (executa imediatamente) |

---

### 1.7 Toast/Notificações - Service Pattern

**Por que criar um ToastService?**

```typescript
// toast.service.ts
@Injectable({ providedIn: 'root' })
export class ToastService {
  private toastSubject = new BehaviorSubject<Toast | null>(null);
  public toast$ = this.toastSubject.asObservable();

  success(message: string, duration: number = 3000) {
    this.toastSubject.next({ message, type: 'success', duration });
    setTimeout(() => this.hide(), duration);
  }

  hide() {
    this.toastSubject.next(null);
  }
}
```

**Uso:**
```typescript
// livro-form.ts
constructor(private toastService: ToastService) {}

onSubmit() {
  this.livroService.create(data).subscribe({
    next: () => {
      this.toastService.success('Livro criado!');
      this.router.navigate(['/livros']);
    },
    error: () => {
      this.toastService.error('Erro ao criar livro');
    }
  });
}
```

**Por que BehaviorSubject?**

- ✅ **Mantém último valor** → Novos subscribers recebem valor atual
- ✅ **Broadcast** → Múltiplos componentes podem ouvir
- ❌ Alternativa: `Subject` (não mantém valor)

---

### 1.8 Roteamento e Lazy Loading

```typescript
// app.routes.ts
export const routes: Routes = [
  { path: '', redirectTo: '/livros', pathMatch: 'full' },
  { path: 'livros', component: LivroList },
  { path: 'livros/novo', component: LivroForm },
  { path: 'livros/editar/:id', component: LivroForm },
  // ... outras rotas
];
```

**Standalone Components + Router:**
- ✅ Sem `RouterModule.forRoot()` → Mais simples
- ✅ `provideRouter(routes)` no `app.config.ts`

**Navegação programática:**
```typescript
constructor(private router: Router, private route: ActivatedRoute) {}

// Navegar
this.router.navigate(['/livros']);

// Pegar parâmetro da URL
const id = this.route.snapshot.paramMap.get('id');
```

---

### 1.9 Integração com API - Environments

```typescript
// environment.ts (desenvolvimento)
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};

// environment.prod.ts (produção/Docker)
export const environment = {
  production: true,
  apiUrl: '/api' // Nginx faz proxy
};
```

**Build de produção:**
```bash
ng build --configuration production
```
- ✅ Usa `environment.prod.ts`
- ✅ Minificação
- ✅ Tree-shaking
- ✅ AOT Compilation

---

### 1.10 Relatório PDF - jsPDF + AutoTable

```typescript
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

exportarPDF(): void {
  const doc = new jsPDF();
  
  doc.setFontSize(18);
  doc.text('Relatório de Livros', 14, 22);
  
  const tableData = this.relatorio().map(item => [
    item.autorNome,
    item.livroTitulo,
    item.editora,
    // ...
  ]);

  autoTable(doc, {
    head: [['Autor', 'Título', 'Editora']],
    body: tableData,
    startY: 35,
    styles: { fontSize: 8 },
    headStyles: { fillColor: [52, 58, 64] }
  });

  doc.save(`relatorio-${Date.now()}.pdf`);
}
```

**Por que jsPDF?**
- ✅ Mais popular (16k stars GitHub)
- ✅ AutoTable plugin para tabelas
- ❌ Alternativa: pdfmake (mais pesado, sintaxe complexa)

---

### 1.11 Bootstrap 5 - Por que não Material ou PrimeNG?

**Escolha:** Bootstrap 5 + Bootstrap Icons

**Justificativa:**
- ✅ **Leve** → 25KB minified + gzipped
- ✅ **Flexível** → Grid system, utilities
- ✅ **Familiar** → 99% dos devs conhecem
- ❌ Material: Muito opinativo, bundle maior
- ❌ PrimeNG: Licença comercial para temas premium

**Uso no projeto:**
```html
<!-- Grid responsivo -->
<div class="row">
  <div class="col-md-8">...</div>
  <div class="col-md-4">...</div>
</div>

<!-- Utilities -->
<div class="d-flex justify-content-between align-items-center">
  <h4 class="mb-0">Livros</h4>
  <button class="btn btn-primary">Novo</button>
</div>
```

---

## 🔶 PARTE 2: BACKEND - ENTITY FRAMEWORK CORE

### 2.1 Arquitetura Clean Architecture + Ports & Adapters

```
Livro.Domain/          # ← Regras de negócio (sem dependências externas)
├── Entity/            # Entidades de domínio
├── Port/              # Interfaces (contratos)
└── Models/            # DTOs

Livro.Application/     # ← Casos de uso (orquestração)
└── UseCase/

Livro.Infra.EfCore/    # ← Adapters (implementação dos Ports)
├── Adapter/           # Implementa as interfaces do Domain
├── Entities/          # Entidades EF Core (mapeamento)
├── Configurations/    # Fluent API
├── Contexts/          # DbContext
└── Migrations/
```

**Princípio da Inversão de Dependência (DIP):**
```
Domain (Port) ← Application ← Infra (Adapter)
     ↑                            ↓
   Interface               Implementação
```

---

### 2.2 DbContext - Configuração Central

```csharp
// AppDbContext.cs
public class AppDbContext : DbContext
{
    public DbSet<LivroEntity> Livros { get; set; }
    public DbSet<AutorEntity> Autores { get; set; }
    public DbSet<AssuntoEntity> Assuntos { get; set; }
    // ... outras entidades

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas as configurações (Fluent API)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```

**Por que `ApplyConfigurationsFromAssembly`?**
- ✅ Descobre automaticamente todas as classes `IEntityTypeConfiguration<T>`
- ✅ Evita código duplicado no `OnModelCreating`
- ✅ Separação de responsabilidades (cada entidade tem sua configuração)

---

### 2.3 Fluent API - Configuração por Entidade

```csharp
// LivroConfiguration.cs
public class LivroConfiguration : IEntityTypeConfiguration<LivroEntity>
{
    public void Configure(EntityTypeBuilder<LivroEntity> builder)
    {
        builder.ToTable("Livro");
        
        builder.HasKey(x => x.Codl);
        
        builder.Property(x => x.Codl)
            .HasColumnName("Codl")
            .HasMaxLength(26)
            .IsRequired();
        
        builder.Property(x => x.Titulo)
            .HasMaxLength(40)
            .IsRequired();
        
        // Relacionamento 1:N (Livro → LivroAutor)
        builder.HasMany(x => x.Autores)
            .WithOne(x => x.Livro)
            .HasForeignKey(x => x.Livro_Codl)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

**Por que Fluent API e não Data Annotations?**

| Fluent API | Data Annotations |
|------------|------------------|
| ✅ Separação domínio/persistência | ❌ Polui entidade |
| ✅ Configurações complexas (índices compostos) | ❌ Limitado |
| ✅ Melhor para DDD | ❌ Mix de conceitos |

---

### 2.4 ULIDs - Por que não GUID/UUID?

**ULID = Universally Unique Lexicographically Sortable Identifier**

```
01ARZ3NDEKTSV4RRFFQ69G5FAV
└─┬─┘ └───────┬───────┘
  │          Random
Timestamp (ms)
```

**Vantagens sobre GUID:**

| ULID | GUID |
|------|------|
| ✅ **Ordenável** (timestamp embutido) | ❌ Random (índices fragmentados) |
| ✅ **26 caracteres** (mais curto em string) | ❌ 36 caracteres |
| ✅ **Case-insensitive** | ❌ Hífen complica parsing |
| ✅ **Performance** em índices B-Tree | ❌ Index fragmentation |

**Uso no projeto:**
```csharp
// Domain
public class LivroDomain
{
    public Ulid Codl { get; set; } = Ulid.NewUlid();
}

// EF Core Entity
public class LivroEntity
{
    public string Codl { get; set; } // Stored as string(26)
}

// Conversão no Adapter
var entity = new LivroEntity 
{ 
    Codl = domain.Codl.ToString() 
};
```

---

### 2.5 Migrations - Code First

```bash
# Criar migration
dotnet ef migrations add NomeMigration

# Aplicar no banco
dotnet ef database update
```

**Migrations criadas:**
1. `MigrateToUlid` → Criou tabelas com ULIDs
2. `AddRelatorioView` → Criou VIEW para relatório
3. `FixRelatorioViewAssuntos` → Corrigiu agregação de assuntos

**Por que Code First?**
- ✅ **Histórico versionado** (migrations no Git)
- ✅ **Reproduzível** (qualquer dev roda `update-database`)
- ✅ **CI/CD friendly**
- ❌ Database First: Dificulta versionamento, gera código repetitivo

---

### 2.6 VIEW no Banco - Relatório

```csharp
// Migration: AddRelatorioView.cs
migrationBuilder.Sql(@"
    CREATE VIEW vw_RelatorioLivros AS
    SELECT 
        a.Nome AS AutorNome,
        l.Titulo AS LivroTitulo,
        l.Editora,
        l.Edicao,
        l.AnoPublicacao,
        GROUP_CONCAT(DISTINCT ass.Descricao) AS Assuntos,
        SUM(lv.Valor) AS ValorTotal
    FROM Livro l
    INNER JOIN Livro_Autor la ON l.Codl = la.Livro_Codl
    INNER JOIN Autor a ON la.Autor_CodAu = a.CodAu
    LEFT JOIN Livro_Assunto las ON l.Codl = las.Livro_Codl
    LEFT JOIN Assunto ass ON las.Assunto_CodAs = ass.CodAs
    LEFT JOIN Livro_Valor lv ON l.Codl = lv.Livro_Codl
    GROUP BY a.Nome, l.Titulo, l.Editora, l.Edicao, l.AnoPublicacao
    ORDER BY a.Nome, l.Titulo
");
```

**Por que VIEW e não query direta?**
- ✅ **Encapsulamento** → Lógica de relatório no banco
- ✅ **Performance** → SQLite otimiza VIEW
- ✅ **Reutilizável** → Pode ser usada por outras queries
- ✅ **Manutenibilidade** → Mudança na VIEW não afeta código

**Mapeamento no EF Core:**
```csharp
// Configurations/RelatorioLivroConfiguration.cs
builder.ToView("vw_RelatorioLivros");
builder.HasNoKey(); // VIEW não tem chave primária
```

---

### 2.7 Seed Data - Dados Iniciais

```csharp
// Seeds/TipoCompraSeed.cs
public class TipoCompraSeed : ISeed
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoCompraEntity>().HasData(
            new { CodTc = Ulid.NewUlid().ToString(), Descricao = "Balcão" },
            new { CodTc = Ulid.NewUlid().ToString(), Descricao = "Self-Service" },
            new { CodTc = Ulid.NewUlid().ToString(), Descricao = "Internet" },
            new { CodTc = Ulid.NewUlid().ToString(), Descricao = "Evento" }
        );
    }
}
```

**Aplicação:**
```csharp
// Program.cs
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
context.Database.Migrate(); // Aplica migrations
DatabaseSeeder.Seed(context); // Aplica seeds
```

---

### 2.8 Repository Pattern (Adapter)

```csharp
// Domain/Port/Livro/Read/GetAllLivros/IGetAllLivrosPort.cs
public interface IGetAllLivrosPort
{
    Task<ResultDetail<List<LivroDomain>>> ExecuteAsync();
}

// Infra/Adapter/Livro/Read/GetAllLivros/GetAllLivrosPortAdapter.cs
public class GetAllLivrosPortAdapter : IGetAllLivrosPort
{
    private readonly AppDbContext _context;

    public async Task<ResultDetail<List<LivroDomain>>> ExecuteAsync()
    {
        var entities = await _context.Livros
            .Include(x => x.Autores).ThenInclude(x => x.Autor)
            .Include(x => x.Assuntos).ThenInclude(x => x.Assunto)
            .Include(x => x.Valores).ThenInclude(x => x.TipoCompra)
            .ToListAsync();

        var domains = entities.Select(EntityExtensions.ToDomain).ToList();
        return domains.GetResultDetailSuccess();
    }
}
```

**Padrão Include (Eager Loading):**
- ✅ **Menos queries** → Carrega tudo de uma vez
- ❌ Pode trazer dados desnecessários (use `Select` para projeção)

**Alternativas:**
- **Lazy Loading** → `virtual` properties (N+1 problem)
- **Explicit Loading** → `_context.Entry(entity).Collection(x => x.Autores).Load()`

---

### 2.9 Conversão Entity ↔ Domain

```csharp
// EntityExtensions.cs
public static LivroDomain ToDomain(this LivroEntity entity)
{
    return new LivroDomain
    {
        Codl = Ulid.Parse(entity.Codl),
        Titulo = entity.Titulo,
        Editora = entity.Editora,
        Edicao = entity.Edicao,
        AnoPublicacao = entity.AnoPublicacao,
        ListAutor = entity.Autores?.Select(x => x.Autor.ToDomain()).ToList(),
        ListAssunto = entity.Assuntos?.Select(x => x.Assunto.ToDomain()).ToList(),
        ListLivroValor = entity.Valores?.Select(x => x.ToDomain()).ToList()
    };
}

public static LivroEntity ToEntity(this LivroDomain domain)
{
    return new LivroEntity
    {
        Codl = domain.Codl.ToString(),
        Titulo = domain.Titulo,
        // ...
    };
}
```

**Por que essa separação?**
- ✅ **Domain** não conhece EF Core (sem [Column], [Table])
- ✅ **Testabilidade** → Mock fácil do domínio
- ✅ **Flexibilidade** → Trocar ORM não afeta domínio

---

### 2.10 SQLite - Por que não SQL Server/PostgreSQL?

**Justificativa:**

| SQLite | SQL Server |
|--------|------------|
| ✅ **Zero configuração** | ❌ Precisa instalar servidor |
| ✅ **Portátil** (arquivo único) | ❌ Servidor dedicado |
| ✅ **Perfeito para dev/testes** | ❌ Overhead para projetos pequenos |
| ❌ Sem suporte a concorrência alta | ✅ Escalável |

**Quando migrar para SQL Server/PostgreSQL?**
- 🔴 Concorrência > 100 writes/segundo
- 🔴 Banco > 140TB (limite teórico do SQLite)
- 🔴 Múltiplos servidores (replicação)

**Configuração:**
```csharp
// appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=../../../data/livro.db"
}

// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

---

## 📁 PARTE 3: ARQUIVOS DE CONFIGURAÇÃO - EXPLICAÇÃO COMPLETA

### 3.1 Estrutura do Projeto Angular

```
livro_presentation_angular/
├── .dockerignore          # ← Exclui arquivos desnecessários do Docker
├── Dockerfile             # ← Multi-stage build (build + nginx)
├── nginx.conf             # ← Proxy reverso + servidor estático
└── livro-app/
    ├── .angular/          # ← Cache de build (gerado automaticamente)
    ├── .vscode/           # ← Configurações do VS Code (opcional)
    ├── .editorconfig      # ← Padronização de código entre editores
    ├── .gitignore         # ← Arquivos ignorados pelo Git
    ├── angular.json       # ← Configuração principal do Angular CLI
    ├── package.json       # ← Dependências NPM + scripts
    ├── package-lock.json  # ← Lock de versões (gerado automaticamente)
    ├── tsconfig.json      # ← Configuração TypeScript (global)
    ├── tsconfig.app.json  # ← TypeScript para aplicação
    ├── tsconfig.spec.json # ← TypeScript para testes
    ├── node_modules/      # ← Dependências instaladas (ignorado no Git)
    ├── dist/              # ← Build de produção (gerado)
    ├── public/            # ← Arquivos estáticos (favicon, etc)
    └── src/               # ← Código-fonte da aplicação
```

---

### 3.2 Arquivo por Arquivo - Explicação Detalhada

#### 📄 `.dockerignore`

**O que faz:** Evita copiar arquivos desnecessários para a imagem Docker.

```dockerignore
node_modules/   # Não copia dependências (npm install será rodado dentro do container)
dist/           # Não copia builds antigos
.angular/       # Cache do Angular (será recriado)
coverage/       # Relatórios de testes
*.log           # Logs
```

**Por que é importante:**
- ✅ **Reduz tamanho da imagem** (de ~500MB para ~50MB)
- ✅ **Build mais rápido** (menos arquivos para copiar)
- ✅ **Evita conflitos** (node_modules do host ≠ do container)

**Como melhorar:**
```dockerignore
# Adicionar se tiver:
.git/
.env.local
*.spec.ts       # Se não rodar testes no Docker
e2e/            # Testes end-to-end
```

---

#### 📄 `Dockerfile` (Frontend)

**O que faz:** Multi-stage build para otimizar produção.

```dockerfile
# STAGE 1: Build da aplicação
FROM node:20-alpine AS build
WORKDIR /app
COPY livro-app/package*.json ./
RUN npm ci                    # npm ci é mais rápido que npm install
COPY livro-app/ ./
RUN npm run build             # ng build --configuration production

# STAGE 2: Servir com Nginx
FROM nginx:alpine
COPY --from=build /app/dist/livro-app/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

**Por que multi-stage?**

| Abordagem | Tamanho final |
|-----------|---------------|
| ❌ Single-stage (Node + build) | ~900MB |
| ✅ Multi-stage (apenas Nginx + dist) | ~25MB |

**Benefícios:**
- ✅ **Imagem menor** → Deploy mais rápido
- ✅ **Apenas runtime** → Sem ferramentas de build em produção
- ✅ **Segurança** → Menos vetores de ataque

**Como melhorar:**
```dockerfile
# Usar .dockerignore otimizado
# Adicionar cache de dependências
COPY livro-app/package*.json ./
RUN npm ci --only=production  # ← Apenas deps de produção
```

---

#### 📄 `nginx.conf`

**O que faz:** Servidor web + proxy reverso para API.

```nginx
server {
    listen 80;
    root /usr/share/nginx/html;
    index index.html;

    # SPA: todas rotas servem index.html
    location / {
        try_files $uri $uri/ /index.html;
    }

    # Proxy para API (evita CORS)
    location /api/ {
        proxy_pass http://livro-api:8080/api/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }

    # Cache de assets estáticos
    location ~* \.(jpg|jpeg|png|gif|ico|css|js|svg|woff|woff2)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }

    # Gzip compression
    gzip on;
    gzip_types text/plain text/css application/json application/javascript;
}
```

**Por que precisa do proxy `/api/`?**

Sem proxy:
```
Frontend (localhost:4200) → Backend (localhost:5000) ❌ CORS Error
```

Com proxy:
```
Frontend → Nginx (localhost:80/api) → Backend (livro-api:8080) ✅
```

**Como melhorar:**
```nginx
# Adicionar headers de segurança
add_header X-Frame-Options "SAMEORIGIN";
add_header X-Content-Type-Options "nosniff";
add_header X-XSS-Protection "1; mode=block";

# Rate limiting
limit_req_zone $binary_remote_addr zone=api:10m rate=10r/s;
location /api/ {
    limit_req zone=api burst=20;
    proxy_pass http://livro-api:8080/api/;
}
```

---

#### 📄 `package.json`

**O que faz:** Gerencia dependências NPM e scripts de build.

```json
{
  "name": "livro-app",
  "version": "0.0.0",
  "scripts": {
    "ng": "ng",
    "start": "ng serve",              // ← npm start (dev)
    "build": "ng build",              // ← ng build --configuration production
    "watch": "ng build --watch",      // ← Build incremental
    "test": "ng test"                 // ← Vitest
  },
  "dependencies": {
    "@angular/common": "^21.0.0",
    "@angular/core": "^21.0.0",
    "@angular/forms": "^21.0.0",
    "@angular/router": "^21.0.0",
    "bootstrap": "^5.3.8",            // ← UI framework
    "jspdf": "^3.0.4",                // ← PDF export
    "jspdf-autotable": "^5.0.2",      // ← Tabelas no PDF
    "rxjs": "~7.8.0"                  // ← Programação reativa
  },
  "devDependencies": {
    "@angular/build": "^21.0.0",
    "@angular/cli": "^21.0.0",
    "typescript": "~5.9.2",
    "vitest": "^4.0.8"                // ← Testes unitários
  }
}
```

**Por que essas dependências?**

| Lib | Justificativa |
|-----|---------------|
| `bootstrap` | UI pronta, leve (25KB), familiar |
| `jspdf` | Mais popular para PDF client-side |
| `rxjs` | Core do Angular, programação reativa |
| `vitest` | Mais rápido que Karma/Jasmine |

**Como melhorar:**
```json
// Adicionar se precisar:
"dependencies": {
  "ngx-mask": "^18.0.0",           // Máscaras de input (CPF, telefone)
  "chart.js": "^4.0.0",            // Gráficos
  "date-fns": "^3.0.0"             // Manipulação de datas
}
```

---

#### 📄 `angular.json`

**O que faz:** Configuração central do Angular CLI (build, serve, test).

```json
{
  "projects": {
    "livro-app": {
      "architect": {
        "build": {
          "options": {
            "browser": "src/main.ts",          // ← Entry point
            "outputPath": "dist/livro-app",    // ← Pasta de build
            "assets": ["public"],              // ← Arquivos estáticos
            "styles": [
              "node_modules/bootstrap/dist/css/bootstrap.min.css",
              "src/styles.css"
            ],
            "scripts": [
              "node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"
            ]
          },
          "configurations": {
            "production": {
              "budgets": [
                {
                  "type": "initial",
                  "maximumWarning": "1.5MB",   // ← Aviso se > 1.5MB
                  "maximumError": "2MB"        // ← Erro se > 2MB
                }
              ],
              "outputHashing": "all"           // ← Cache busting
            }
          }
        },
        "serve": {
          "options": {
            "port": 4200,
            "host": "localhost"
          }
        }
      }
    }
  }
}
```

**Conceitos importantes:**

- **budgets**: Limita tamanho do bundle (evita bundle bloat)
- **outputHashing**: Adiciona hash aos arquivos (ex: `main.abc123.js`) para invalidar cache
- **assets**: Copia arquivos estáticos sem processamento
- **styles/scripts**: Importa CSS/JS global (fora do bundler do Angular)

**Como melhorar:**
```json
"configurations": {
  "production": {
    "sourceMap": false,              // ← Remove source maps em prod
    "optimization": true,
    "buildOptimizer": true,
    "namedChunks": false,            // ← Nomes genéricos (menor bundle)
    "aot": true,                     // ← Ahead-of-Time compilation
    "extractLicenses": true
  }
}
```

---

#### 📄 `tsconfig.json`

**O que faz:** Configuração global do TypeScript.

```json
{
  "compilerOptions": {
    "strict": true,                           // ← Modo estrito (recomendado)
    "noImplicitReturns": true,               // ← Funções devem retornar algo
    "noFallthroughCasesInSwitch": true,      // ← Switch precisa de break
    "skipLibCheck": true,                     // ← Ignora erros em node_modules
    "experimentalDecorators": true,          // ← Permite @Component, @Injectable
    "target": "ES2022",                      // ← JavaScript alvo
    "module": "preserve"                     // ← Preserva import/export (ESM)
  }
}
```

**Por que `"strict": true`?**

```typescript
// ❌ Sem strict
let nome;
nome = 42;        // OK (any implícito)
nome.toUpperCase(); // Runtime error

// ✅ Com strict
let nome: string;
nome = 42;        // ❌ Erro em tempo de compilação
```

**Como melhorar:**
```json
"compilerOptions": {
  "noUnusedLocals": true,        // ← Erro se variável não for usada
  "noUnusedParameters": true,    // ← Erro se parâmetro não for usado
  "strictNullChecks": true       // ← null/undefined mais seguro
}
```

---

#### 📄 `tsconfig.app.json` e `tsconfig.spec.json`

**O que fazem:** Configurações específicas para app e testes.

```json
// tsconfig.app.json (aplicação)
{
  "extends": "./tsconfig.json",
  "compilerOptions": {
    "outDir": "./out-tsc/app"
  },
  "include": ["src/**/*.ts"],
  "exclude": ["src/**/*.spec.ts"]  // ← Ignora testes
}

// tsconfig.spec.json (testes)
{
  "extends": "./tsconfig.json",
  "compilerOptions": {
    "types": ["vitest/globals"]    // ← Tipagens de teste
  },
  "include": ["src/**/*.spec.ts"]  // ← Apenas testes
}
```

**Por que separar?**
- ✅ Build de produção **não inclui** código de teste
- ✅ Testes têm configurações específicas (tipos de teste, etc)

---

#### 📄 `.editorconfig`

**O que faz:** Padroniza formatação entre editores (VS Code, IntelliJ, Vim).

```editorconfig
root = true

[*]
charset = utf-8
indent_style = space
indent_size = 2              # ← 2 espaços (padrão Angular)
insert_final_newline = true
trim_trailing_whitespace = true

[*.ts]
quote_type = single          # ← 'aspas simples' em TypeScript

[*.md]
trim_trailing_whitespace = false  # ← Markdown precisa de espaços
```

**Por que é importante:**
- ✅ **Consistência** → Todo time usa mesma formatação
- ✅ **Git limpo** → Sem conflitos de espaços/tabs
- ✅ **Funciona em qualquer editor** → VS Code, Vim, IntelliJ

---

#### 📄 `.gitignore` (Angular)

**O que faz:** Arquivos que NÃO devem ir pro Git.

```gitignore
# Dependências
/node_modules/

# Build
/dist/
/out-tsc/
/bazel-out/

# Cache
/.angular/cache/

# IDEs
.vscode/*
!.vscode/settings.json   # ← Pode versionar settings compartilhados
!.vscode/tasks.json
.idea/

# Sistema
.DS_Store
Thumbs.db

# Env
.env.local
.env.*.local
```

**Como melhorar:**
```gitignore
# Adicionar:
coverage/           # ← Relatórios de cobertura
*.log
.env               # ← Senhas/tokens
```

---

### 3.3 Docker - Backend (.NET)

#### 📄 `livro_api/Dockerfile`

```dockerfile
# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia .csproj e restaura dependências (cache de layers)
COPY ["src/Livro.Presentation.Api/Livro.Presentation.Api.csproj", "src/Livro.Presentation.Api/"]
RUN dotnet restore "src/Livro.Presentation.Api/Livro.Presentation.Api.csproj"

# Copia código e compila
COPY . .
WORKDIR "/src/src/Livro.Presentation.Api"
RUN dotnet build "Livro.Presentation.Api.csproj" -c Release -o /app/build

# Publica
FROM build AS publish
RUN dotnet publish "Livro.Presentation.Api.csproj" -c Release -o /app/publish

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Livro.Presentation.Api.dll"]
```

**Por que multi-stage?**

| Imagem | Tamanho |
|--------|---------|
| `dotnet/sdk:10.0` (build) | ~700MB |
| `dotnet/aspnet:10.0` (runtime) | ~200MB |

**Como funciona o cache de layers:**

```dockerfile
# ✅ BOM: Copia .csproj primeiro (muda raramente)
COPY *.csproj ./
RUN dotnet restore

# DEPOIS copia código (muda frequentemente)
COPY . .
```

Se o código mudar mas o `.csproj` não, o `dotnet restore` usa cache → Build 5x mais rápido.

---

#### 📄 `docker-compose.yml`

**O que faz:** Orquestra múltiplos containers (API + Frontend + Network).

```yaml
services:
  livro-api:
    build:
      context: ./livro_api
      dockerfile: Dockerfile
    container_name: livro-api
    ports:
      - "5000:8080"           # ← Host:Container
    volumes:
      - ./data:/app/data      # ← Persiste banco SQLite
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    networks:
      - livro-network

  livro-web:
    build:
      context: ./livro_presentation_angular
      dockerfile: Dockerfile
    container_name: livro-web
    ports:
      - "4200:80"
    depends_on:
      - livro-api             # ← Aguarda API subir
    networks:
      - livro-network

networks:
  livro-network:
    driver: bridge            # ← Rede interna Docker
```

**Conceitos importantes:**

- **volumes**: Compartilha pasta entre host e container (banco persiste após parar container)
- **depends_on**: Ordem de inicialização (mas não aguarda health check)
- **networks**: Containers na mesma rede se comunicam por nome (`livro-api:8080`)

**Como melhorar:**
```yaml
services:
  livro-api:
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
      interval: 30s
      timeout: 10s
      retries: 3
    restart: unless-stopped   # ← Reinicia automaticamente se crashar
```

---

### 3.4 Arquivos NÃO Utilizados (Podem Ser Removidos)

#### ❌ `.vscode/` (pasta)
- **O que é:** Configurações específicas do VS Code
- **Usado?** Opcional, pode commitar se quiser compartilhar settings com o time
- **Melhor prática:** Adicionar ao `.gitignore` ou commitar apenas `settings.json` compartilhado

#### ❌ `.angular/` (pasta)
- **O que é:** Cache de build do Angular
- **Usado?** Sim, mas auto-gerado
- **Deve commitar?** NÃO, já está no `.gitignore`

#### ❌ `dist/` (pasta)
- **O que é:** Build de produção
- **Usado?** Apenas localmente ou no Docker
- **Deve commitar?** NÃO, é gerado

#### ❌ `node_modules/` (pasta)
- **O que é:** Dependências NPM
- **Usado?** Sim, mas baixado via `npm install`
- **Deve commitar?** NUNCA (150k+ arquivos)

---

### 3.5 Checklist de Configurações

| Arquivo | Status | Pode Melhorar? |
|---------|--------|----------------|
| `.dockerignore` | ✅ Configurado | Adicionar `.git/`, `*.spec.ts` |
| `Dockerfile` (frontend) | ✅ Multi-stage | Usar `npm ci --only=production` |
| `Dockerfile` (backend) | ✅ Multi-stage | Adicionar healthcheck |
| `nginx.conf` | ✅ Proxy + cache | Headers de segurança, rate limiting |
| `docker-compose.yml` | ✅ Funcional | Healthcheck, restart policy |
| `package.json` | ✅ Completo | OK |
| `angular.json` | ✅ Completo | Ajustar budgets se precisar |
| `tsconfig.json` | ✅ Strict mode | `noUnusedLocals`, `noUnusedParameters` |
| `.editorconfig` | ✅ Padronizado | OK |
| `.gitignore` | ✅ Completo | Adicionar `coverage/` |

---

## 🎓 PERGUNTAS QUE PODEM CAIR

### Angular

**P1: Por que Signals ao invés de NgRx?**

R: Signals são a solução nativa do Angular 16+ para reatividade. Para este CRUD, NgRx seria over-engineering. Signals oferecem:
- Sintaxe mais simples (`livros.set([])` vs `dispatch(loadLivros())`)
- Sem boilerplate (actions, reducers, effects)
- Performance superior (change detection granular)
- Recomendado pelo time Angular para estado local

NgRx seria necessário em apps com estado global complexo, time travel debugging, ou múltiplos módulos compartilhando estado.

---

**P2: Como funciona o Change Detection com Signals?**

R: Signals implementam **fine-grained reactivity**. Quando você faz `livros.set(novoValor)`, apenas os componentes que **lêem** `livros()` são atualizados, não a árvore inteira. Isso é mais eficiente que:
- **Default CD**: Verifica toda árvore a cada evento
- **OnPush CD**: Verifica apenas se `@Input()` muda

---

**P3: Por que Standalone Components?**

R: É o futuro do Angular (padrão desde v14). Vantagens:
- Bundle menor (tree-shaking melhor)
- Menos código (sem `@NgModule`)
- Lazy loading simplificado
- Migração gradual possível

---

**P4: Como você lida com erros HTTP?**

R: RxJS `catchError` no service:
```typescript
catchError((error) => {
  if (error.status === 404) return of([]); // Trata 404 como lista vazia
  return throwError(() => error); // Re-lança outros erros
})
```

No componente, `subscribe` com `error` callback:
```typescript
this.service.getAll().subscribe({
  next: (data) => { /* sucesso */ },
  error: (err) => { 
    this.toastService.error('Erro');
    console.error(err);
  }
});
```

---

### Entity Framework Core

**P5: Explique o relacionamento N:N entre Livro e Autor**

R: EF Core 5+ suporta N:N implícito, mas usei **tabela de junção explícita** (`Livro_Autor`) para ter controle total:

```csharp
// LivroAutorConfiguration.cs
builder.HasKey(x => new { x.Livro_Codl, x.Autor_CodAu }); // Chave composta

builder.HasOne(x => x.Livro)
    .WithMany(x => x.Autores)
    .HasForeignKey(x => x.Livro_Codl);

builder.HasOne(x => x.Autor)
    .WithMany(x => x.Livros)
    .HasForeignKey(x => x.Autor_CodAu);
```

Vantagens:
- Posso adicionar colunas na tabela de junção (ex: `DataCriacao`, `Ordem`)
- Controle sobre índices

---

**P6: Por que Include() ao invés de Lazy Loading?**

R: **Lazy Loading** causa o problema N+1:
```csharp
// ❌ Lazy Loading
var livros = context.Livros.ToList(); // 1 query
foreach(var livro in livros) {
    Console.WriteLine(livro.Autores.Count); // N queries (uma por livro)
}
```

**Eager Loading** com `Include()` resolve isso:
```csharp
// ✅ Eager Loading
var livros = context.Livros
    .Include(x => x.Autores)
    .ToList(); // 1 query com JOIN
```

---

**P7: Como funciona o padrão Repository (Port/Adapter)?**

R: **Inversão de Dependência**:
1. **Domain** define a interface (`IGetAllLivrosPort`)
2. **Application** depende da interface
3. **Infra** implementa (`GetAllLivrosPortAdapter`)
4. **DI Container** injeta implementação em runtime

Benefícios:
- Domain não conhece EF Core
- Trocar ORM não afeta lógica de negócio
- Testes unitários mockam o Port

```csharp
// Application/UseCase
public class GetAllLivrosUseCase
{
    private readonly IGetAllLivrosPort _port; // ← Interface

    public GetAllLivrosUseCase(IGetAllLivrosPort port) 
    {
        _port = port; // Injetado via DI
    }
}
```

---

**P8: Explique o Dockerfile multi-stage do Frontend**

R: Multi-stage build reduz drasticamente o tamanho da imagem final:

**STAGE 1 (Build):**
```dockerfile
FROM node:20-alpine AS build  # ← Imagem completa com Node.js
WORKDIR /app
COPY livro-app/package*.json ./
RUN npm ci                    # ← Instala dependências
COPY livro-app/ ./
RUN npm run build             # ← ng build --prod (~2min)
```

**STAGE 2 (Runtime):**
```dockerfile
FROM nginx:alpine             # ← Apenas Nginx (25MB)
COPY --from=build /app/dist/livro-app/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
```

Resultado:
- ❌ Single-stage: ~900MB (Node + deps + build tools)
- ✅ Multi-stage: ~25MB (Nginx + HTML/CSS/JS compilados)

---

**P9: Como funciona o proxy reverso no nginx.conf?**

R: O nginx atua como **gateway único** que:

1. **Serve o frontend** (HTML, CSS, JS)
```nginx
location / {
    try_files $uri $uri/ /index.html;  # ← SPA: redireciona tudo para index.html
}
```

2. **Faz proxy para API** (evita CORS)
```nginx
location /api/ {
    proxy_pass http://livro-api:8080/api/;  # ← Encaminha para backend
}
```

**Fluxo:**
```
Browser → http://localhost:4200/api/livro
    ↓
Nginx recebe em /api/livro
    ↓
Proxy para http://livro-api:8080/api/livro (rede interna Docker)
    ↓
Backend responde
    ↓
Nginx devolve pro Browser
```

**Vantagens:**
- ✅ **Sem CORS** (tudo vem do mesmo domínio)
- ✅ **SSL Termination** (pode adicionar HTTPS no nginx)
- ✅ **Load balancing** (pode distribuir para múltiplos backends)
- ✅ **Cache** (assets estáticos com `expires 1y`)

---

**P10: Por que usar npm ci ao invés de npm install no Docker?**

R: `npm ci` (Clean Install) é otimizado para CI/CD:

| `npm install` | `npm ci` |
|---------------|----------|
| ❌ Lê `package.json` e atualiza `package-lock.json` | ✅ Lê `package-lock.json` (versões exatas) |
| ❌ Pode instalar versões diferentes | ✅ Reproduzível (mesmas versões sempre) |
| ❌ Mais lento | ✅ 2-3x mais rápido |
| ❌ Mantém `node_modules` existente | ✅ Deleta e recria do zero |

```dockerfile
COPY package*.json ./
RUN npm ci --only=production  # ← Apenas deps de prod, sem devDependencies
```

---

**P11: O que são os budgets no angular.json?**

R: **Budgets** limitam o tamanho do bundle para evitar apps lentas:

```json
"budgets": [
  {
    "type": "initial",
    "maximumWarning": "1.5MB",   // ← Aviso amarelo
    "maximumError": "2MB"        // ← Build falha
  }
]
```

**Por que é importante?**
- ❌ Bundle grande → Loading lento → Usuários desistem
- ✅ Força otimização (lazy loading, tree-shaking)
- ✅ Detecta bibliotecas pesadas acidentalmente adicionadas

**Como resolver se estourar:**
```bash
# Analisa o bundle
ng build --stats-json
npx webpack-bundle-analyzer dist/livro-app/stats.json
```

Soluções:
- Lazy load de features
- Remover bibliotecas não usadas
- Usar imports específicos: `import { map } from 'rxjs/operators'` ao invés de `import * as rxjs`

---

**P12: Como a VIEW é criada no banco?**

R: Via **Migration** com SQL raw:
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"CREATE VIEW vw_RelatorioLivros AS ...");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql("DROP VIEW vw_RelatorioLivros");
}
```

Mapeamento no EF Core:
```csharp
modelBuilder.Entity<RelatorioLivroEntity>()
    .ToView("vw_RelatorioLivros")
    .HasNoKey(); // VIEWs não têm PK
```

---

## 📊 Comparações Técnicas

### Angular vs React vs Vue

| Recurso | Angular | React | Vue |
|---------|---------|-------|-----|
| **Estado** | Signals (nativo) | useState/useReducer | ref/reactive (Composition API) |
| **Forms** | Reactive Forms | Controlled components | v-model |
| **DI** | Nativo (`@Injectable`) | Context API / libs | provide/inject |
| **Routing** | @angular/router | React Router | Vue Router |
| **CLI** | Angular CLI | Create React App / Vite | Vue CLI / Vite |

**Quando usar Angular?**
- Apps empresariais (TypeScript obrigatório)
- Times grandes (opinionated)
- Integração com RxJS

---

### EF Core vs Dapper vs ADO.NET

| ORM | Quando usar |
|-----|------------|
| **EF Core** | CRUD padrão, migrations, relacionamentos |
| **Dapper** | Queries complexas, performance crítica |
| **ADO.NET** | Legacy, controle total sobre SQL |

**Por que EF Core neste projeto?**
- ✅ Migrations versionadas
- ✅ Change tracking
- ✅ Relacionamentos automáticos (Include)

---

## 🚀 Comandos Importantes

### Angular
```bash
# Desenvolvimento
ng serve

# Build de produção
ng build --configuration production

# Testes
ng test

# Criar componente
ng generate component features/livro/livro-list --standalone
```

### .NET + EF Core
```bash
# Rodar API
dotnet run

# Criar migration
dotnet ef migrations add NomeMigration

# Aplicar migrations
dotnet ef database update

# Testes
dotnet test
```

### Docker
```bash
# Subir tudo
docker-compose up --build

# Parar
docker-compose down

# Ver logs
docker-compose logs -f livro-api
```

---

## 🎯 Resumo Executivo

**Frontend Angular:**
- ✅ Standalone Components (Angular moderno)
- ✅ Signals para estado (reatividade nativa)
- ✅ Reactive Forms (validação robusta)
- ✅ Services com RxJS (programação reativa)
- ✅ Bootstrap 5 (UI leve e responsiva)
- ✅ jsPDF para relatórios

**Backend .NET:**
- ✅ Clean Architecture + Ports & Adapters
- ✅ EF Core com Fluent API
- ✅ ULIDs (performance e ordenação)
- ✅ SQLite (portabilidade)
- ✅ Migrations versionadas
- ✅ VIEW para relatório complexo

**Diferenciais:**
- ✅ Arquitetura escalável e testável
- ✅ Separação de conceitos (Domain, Application, Infra)
- ✅ Docker pronto para produção
- ✅ TypeScript + C# (tipagem forte)

---

## 📚 Material de Apoio

**Documentação Oficial:**
- Angular Signals: https://angular.dev/guide/signals
- Angular Standalone: https://angular.dev/guide/components/importing
- EF Core Fluent API: https://learn.microsoft.com/ef/core/modeling/
- Clean Architecture: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

**Leitura Recomendada:**
- "Clean Architecture" - Robert C. Martin
- "Domain-Driven Design" - Eric Evans
- RxJS Documentation: https://rxjs.dev/

---

**Boa sorte na avaliação! 🚀**
