using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.AutoMapper;

namespace Utilitario.ParaTestes.Mapper;

public class MapperBuilder
{
    public static IMapper Instancia()
    {
        var configuracao = new MapperConfiguration(opt =>
        {
            opt.AddProfile<AutoMapperConfiguration>();
        });

        return configuracao.CreateMapper();
    }
}