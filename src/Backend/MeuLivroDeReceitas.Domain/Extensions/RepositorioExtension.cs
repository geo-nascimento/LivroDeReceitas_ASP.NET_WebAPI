using Microsoft.Extensions.Configuration;

namespace MeuLivroDeReceitas.Domain.Extensions;

public static class RepositorioExtension
{
    public static string GetNomeDatabase(this IConfiguration configuration)
    {
        var databaseName = configuration.GetConnectionString("DatabaseName");

        return databaseName;
    }
    
    public static string GetConexao(this IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Conexao");

        return connectionString;
    }

    public static string GetFullConnectionString(this IConfiguration configuration)
    {
        var conexao = configuration.GetConexao();
        var nomeDatabase = configuration.GetNomeDatabase();

        return $"{conexao}Database={nomeDatabase}";
    }
}