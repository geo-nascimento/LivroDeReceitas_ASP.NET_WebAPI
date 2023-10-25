using System.Reflection;
using FluentMigrator.Runner;
using MeuLivroDeReceitas.Domain.Extensions;
using MeuLivroDeReceitas.Domain.Repositories;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infrastructure;

public static class Bootstrapper
{
    public static void AddRepositorio(this IServiceCollection services, IConfiguration configuration)
    {
        AddFluentMigrator(services, configuration);
        AddRepositories(services);
        AddUnitOfWork(services);
        AddContextData(services, configuration);
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        bool.TryParse(configuration.GetSection("ConfiguracoesTeste:BacoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);

        if (!bancoDeDadosInMemory)
        {
            services
            .AddFluentMigratorCore()
            .ConfigureRunner(c => c.AddMySql5()
                .WithGlobalConnectionString(configuration.GetFullConnectionString())
                .ScanIn(Assembly.Load("MeuLivroDeReceitas.Infrastructure")).For.All());
        }
            
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUsuarioReadOnly, UsuarioRepositorio>()
                .AddScoped<IUsuarioWriteOnly, UsuarioRepositorio>()
                .AddScoped<IUsuarioUpdateOnly, UsuarioRepositorio>();
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddContextData(IServiceCollection service, IConfiguration configuration)
    {
        bool.TryParse(configuration.GetSection("ConfiguracoesTeste:BancoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);

        if (!bancoDeDadosInMemory)
        {
            service
           .AddDbContext<LivroDeReceitasContext>(opt =>
               opt.UseMySql(configuration.GetFullConnectionString(),
                   ServerVersion.AutoDetect(configuration.GetFullConnectionString())));
        }

       
    }
}