using Livro.Infra.EfCore.Contexts;
using Livro.Infra.EfCore.Seeds;
using Microsoft.EntityFrameworkCore;

namespace Livro.Presentation.Api.Extensions;

/// <summary>
/// Extension methods para inicialização automática do banco de dados.
/// </summary>
public static class DatabaseInitializerExtensions
{
    /// <summary>
    /// Inicializa o banco de dados: cria/migra schema e executa seeds automaticamente.
    /// Chame após app.Build() no Program.cs.
    /// </summary>
    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetService<ILogger<AppDbContext>>();

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            
            // Cria/migra o banco de dados aplicando migrations pendentes
            logger?.LogInformation("🗄️  Verificando banco de dados...");
            await context.Database.MigrateAsync();
            logger?.LogInformation("✅ Banco de dados pronto (migrations aplicadas)");
            
            // Executa os Seeds automaticamente
            await DatabaseSeeder.SeedAsync(services);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "❌ Erro ao inicializar banco de dados");
            throw;
        }

        return app;
    }
}
