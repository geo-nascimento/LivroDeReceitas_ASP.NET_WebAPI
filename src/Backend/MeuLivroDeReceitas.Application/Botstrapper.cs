using MeuLivroDeReceitas.Application.Servicos.Critografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Application;

public static class Botstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AdicionarUseCases(services);
        AdicionarTokenJwt(services, configuration);
        AdicionarChaveAdcionalSenha(services, configuration);
        
    }

    private static void AdicionarChaveAdcionalSenha(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(option => new EncriptadorDeSenha(configuration["Configuracoes:ChaveAdicionalSenha"]));
    }

    public static void AdicionarTokenJwt(IServiceCollection services, IConfiguration configuration)
    {
        var tempoDeVidaToken = double.Parse(configuration["Configuracoes:TempoDeVidaDoToken"]);
        var chaveDetoken = configuration["Configuracoes:ChaveDeToken"];
        services.AddScoped(option => new TokenController(tempoDeVidaToken, chaveDetoken));
    }

    private static void AdicionarUseCases(IServiceCollection services)
    {
        services
            .AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>();
    }
}