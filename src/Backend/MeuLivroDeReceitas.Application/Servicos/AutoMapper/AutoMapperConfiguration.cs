using AutoMapper;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Domain.Entities;

namespace MeuLivroDeReceitas.Application.Servicos.AutoMapper;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<RequestRegistrasUsuarioJson, Usuario>()
            .ForMember(a => a.Senha, opt => 
                opt.Ignore()); //Pois resposta da senha vai ser um toquen criptografado
    }
}