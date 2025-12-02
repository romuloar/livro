# Padrão de Seeds - Livro API

## 📋 Visão Geral

Sistema de Seeds inteligente com **descoberta automática via Reflection** e **verificação de existência** antes de inserir dados.

## 🎯 Características

- ✅ **Descoberta Automática**: Basta criar a classe, ela é descoberta automaticamente
- ✅ **Sem Duplicação**: Verifica se dados já existem antes de inserir
- ✅ **Ordenação Inteligente**: Propriedade `Order` controla ordem de execução (importante para FKs)
- ✅ **Logs Detalhados**: Acompanhe a execução de cada Seed
- ✅ **Zero Configuração**: Não precisa registrar manualmente no `DatabaseSeeder`

## 📁 Estrutura

```
Livro.Infra.EfCore/
├── Seeds/
│   ├── ISeed.cs                  # Interface base
│   ├── DatabaseSeeder.cs         # Executor (usa Reflection)
│   ├── TipoCompraSeed.cs        # Exemplo: Order = 1
│   └── [NovasSeedsAqui].cs      # Adicione aqui!
```

## 🚀 Como Adicionar Nova Seed

### 1️⃣ Crie o arquivo na pasta `Seeds/`

```csharp
using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Livro.Infra.EfCore.Seeds;

public class AutorSeed : ISeed
{
    // Ordem de execução (importante para FKs)
    // TipoCompra = 1, Autor = 2, Livro = 3, LivroAutor = 4, etc.
    public int Order => 2;
    
    public async Task SeedAsync(AppDbContext context)
    {
        // Verifica se já existem dados
        if (await context.Autores.AnyAsync())
            return; // Já existe, não insere
        
        // Insere dados iniciais
        var autores = new[]
        {
            new AutorEntity { Nome = "Machado de Assis" },
            new AutorEntity { Nome = "Clarice Lispector" },
            new AutorEntity { Nome = "Jorge Amado" }
        };
        
        await context.Autores.AddRangeAsync(autores);
        await context.SaveChangesAsync();
    }
}
```

### 2️⃣ Pronto! 🎉

A classe será **automaticamente descoberta** e executada na ordem correta.

## 🔢 Guia de Ordenação (Order)

```
1  → TipoCompra (sem dependências)
2  → Autor, Assunto (sem dependências)
3  → Livro (sem dependências)
10 → LivroAutor (depende de Livro + Autor)
11 → LivroAssunto (depende de Livro + Assunto)
12 → LivroValor (depende de Livro + TipoCompra)
```

**Dica**: Deixe espaços entre os números (1, 2, 10, 11...) para facilitar inserções futuras.

## 🔍 Como Funciona

### DatabaseSeeder (Reflection)

```csharp
// Descobre automaticamente todas as classes ISeed
var seedInstances = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => seedType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
    .Select(t => Activator.CreateInstance(t) as ISeed)
    .OrderBy(s => s!.Order) // ← Ordena pela propriedade Order
    .ToList();

// Executa cada Seed na ordem
foreach (var seed in seedInstances)
{
    await seed.SeedAsync(context);
}
```

### Execução no Program.cs

```csharp
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
    
    // Executa os Seeds (com verificação automática de existência)
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
}
```

## ✅ Benefícios

| Problema Anterior | Solução Atual |
|------------------|---------------|
| HasData() reinsere a cada migration | Verifica existência com `AnyAsync()` |
| Ordem manual difícil de manter | Propriedade `Order` explícita |
| Registrar manualmente cada Seed | Reflection descobre automaticamente |
| Logs genéricos | Logs detalhados por Seed |
| Configuração no AppDbContext | Separação total (Clean Architecture) |

## 📝 Exemplo de Log

```
🌱 Iniciando processo de Seed do banco de dados...
📋 Encontradas 3 classe(s) de Seed
  ▶️  Executando [1] TipoCompraSeed...
  ✅ [1] TipoCompraSeed concluído
  ▶️  Executando [2] AutorSeed...
  ✅ [2] AutorSeed concluído
  ▶️  Executando [10] LivroAutorSeed...
  ✅ [10] LivroAutorSeed concluído
🎉 Processo de Seed concluído com sucesso!
```

## 🎓 Melhores Práticas

1. **Use Order adequadamente**: Respeite dependências de FK
2. **Sempre verifique existência**: Use `AnyAsync()` antes de inserir
3. **Dados imutáveis**: Seeds são para dados de referência, não dados de usuário
4. **Idempotência**: Executar 10x deve ter o mesmo resultado que executar 1x
5. **Transactions**: O EF Core já gerencia, mas pode usar `BeginTransaction()` se necessário
