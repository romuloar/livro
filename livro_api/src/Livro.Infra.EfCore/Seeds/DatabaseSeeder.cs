using Livro.Infra.EfCore.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Livro.Infra.EfCore.Seeds;

/// <summary>
/// Gerenciador central de Seeds com descoberta automática via Reflection.
/// Descobre e executa automaticamente todas as classes que implementam ISeed,
/// respeitando a ordem definida na propriedade Order.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Executa todos os seeds automaticamente descobertos no assembly.
    /// Seeds são ordenados pela propriedade Order (menor = primeiro).
    /// </summary>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetService<ILogger<AppDbContext>>();

        try
        {
            logger?.LogInformation("🌱 Iniciando processo de Seed do banco de dados...");

            // Descobre automaticamente todas as classes ISeed via Reflection
            var seedType = typeof(ISeed);
            var seedInstances = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => seedType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t) as ISeed)
                .Where(s => s != null)
                .OrderBy(s => s!.Order) // Ordena pela propriedade Order
                .ToList();

            if (!seedInstances.Any())
            {
                logger?.LogWarning("⚠️  Nenhuma classe de Seed encontrada");
                return;
            }

            logger?.LogInformation("📋 Encontradas {Count} classe(s) de Seed", seedInstances.Count);

            foreach (var seed in seedInstances)
            {
                var seedName = seed!.GetType().Name;
                logger?.LogInformation("  ▶️  Executando [{Order}] {SeedName}...", seed.Order, seedName);
                
                await seed.SeedAsync(context);
                
                logger?.LogInformation("  ✅ [{Order}] {SeedName} concluído", seed.Order, seedName);
            }

            logger?.LogInformation("🎉 Processo de Seed concluído com sucesso!");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "❌ Erro ao executar Seeds do banco de dados");
            throw;
        }
    }
}

