using MeuLivroDeReceitas.Domain.Entities;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Tests;

public class MeuLivroDeReceitasWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private Usuario _usuario;
    private string _senha;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test").ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(LivroDeReceitasContext));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<LivroDeReceitasContext>(opt =>
            {
                opt.UseInMemoryDatabase("InMemoryDbForTesting");
                opt.UseInternalServiceProvider(provider);
            });

            var servicesProvider = services.BuildServiceProvider();

            using var scope = servicesProvider.CreateScope();
            var scopeServices = scope.ServiceProvider;
            var database = scopeServices.GetRequiredService<LivroDeReceitasContext>();

            database.Database.EnsureDeleted();
            
            //Iniciar o banco em memória já com um dado para poder testar o login de forma correta
            (_usuario, _senha) = ContextSeedInMemory.Seed(database);
        });
    }

    public Usuario RecuperarUsuario()
    {
        return _usuario;
    }
    
    public string RecuperarSenha()
    {
        return _senha;
    }
}
