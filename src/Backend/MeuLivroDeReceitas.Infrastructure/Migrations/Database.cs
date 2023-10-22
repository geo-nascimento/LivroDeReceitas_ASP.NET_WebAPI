using Dapper;
using MySqlConnector;

namespace MeuLivroDeReceitas.Infrastructure.Migrations;

public static class Database
{
    public static void CriarDataBase(string connectionString, string dataBaseName)
    {
        //Essa função tem como objetivo verificar se no SGBD existe um schema de banco de dados com esse nome
       using var myConnection = new MySqlConnection(connectionString);

       var param = new DynamicParameters();
       param.Add("nome", dataBaseName);
       
       var registros = myConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @nome", param);

       if (!registros.Any())
       {
           myConnection.Execute($"CREATE DATABASE {dataBaseName}");
       }
    }
}