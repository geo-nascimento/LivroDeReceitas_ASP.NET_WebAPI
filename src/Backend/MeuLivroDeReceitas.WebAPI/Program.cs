using AutoMapper;
using MeuLivroDeReceitas.Application;
using MeuLivroDeReceitas.Application.Servicos.AutoMapper;
using MeuLivroDeReceitas.Domain.Extensions;
using MeuLivroDeReceitas.Infrastructure;
using MeuLivroDeReceitas.Infrastructure.AcessoRepository;
using MeuLivroDeReceitas.Infrastructure.Migrations;
using MeuLivroDeReceitas.WebAPI.Filtros;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Injeção dos reposiórios
builder.Services.AddRepositorio(builder.Configuration);
//Adicionar o filtro de exceção: garante que qualquer exceção lançada dentro da aoplicação será capturada
builder.Services.AddMvc(c => c.Filters.Add(typeof(FiltroDasExceptions)));
builder.Services.AddScoped<UsuarioAutenticadoAttribute>();
//AutoMapper
builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguration());
}).CreateMapper());
//Adicionar serviços de application
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AtualizarBaseDeDados();

app.Run();


void AtualizarBaseDeDados()
{
    using var serviceScopr = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScopr.ServiceProvider.GetService<LivroDeReceitasContext>();

    bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        var databaseName = builder.Configuration.GetNomeDatabase();
        var connectionString = builder.Configuration.GetConexao();

        Database.CriarDataBase(connectionString, databaseName);

        app.MigrateBancoDeDados();
    }
    
}

public partial class Program{}