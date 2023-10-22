using FluentMigrator.Builders.Create.Table;

namespace MeuLivroDeReceitas.Infrastructure.Migrations;

public static class BaseVersion
{
    public static ICreateTableWithColumnOrSchemaOrDescriptionSyntax InserirColunasPadrao(ICreateTableWithColumnOrSchemaOrDescriptionSyntax tabela)
    {
        tabela
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("DtaCriacao").AsDateTime().NotNullable();
        
        return tabela;
    }
}