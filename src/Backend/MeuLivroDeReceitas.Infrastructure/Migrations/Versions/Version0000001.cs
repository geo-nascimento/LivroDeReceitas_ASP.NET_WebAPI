using FluentMigrator;

namespace MeuLivroDeReceitas.Infrastructure.Migrations.Versions;

[Migration((long)NumeroVersoes.CriarTabelaUsuario, "Cria a tabela usuário")]
public class Version0000001 : Migration
{
    public override void Up()
    {
        var table = BaseVersion.InserirColunasPadrao(Create.Table("Usuarios"));
        table
            .WithColumn("Nome").AsString(100).NotNullable()
            .WithColumn("Email").AsString(100).NotNullable()
            .WithColumn("Senha").AsString(2000).NotNullable()
            .WithColumn("Telefone").AsString(15).NotNullable();
    }

    public override void Down()
    {
        
    }
}