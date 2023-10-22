using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infrastructure.Migrations;

public static class MigrationExtension
{
    public static void MigrateBancoDeDados(this IApplicationBuilder app)
    {
        using var scopo = app.ApplicationServices.CreateScope();
        var runner = scopo.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        
        runner.MigrateUp();
    }
}