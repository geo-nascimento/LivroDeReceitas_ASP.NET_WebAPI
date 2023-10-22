using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Comunication.Request;
using MeuLivroDeReceitas.Comunication.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.WebAPI.Controllers;

public class UsuariosController : MeuLivroDeReceitasController 
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseUsuarioRegistrar), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistrarUsuario([FromServices] IRegistrarUsuarioUseCase useCase, 
        RequestRegistrasUsuarioJson request)
    {
        var resultado = await useCase.Executar(request);

        return Created(string.Empty, resultado);
    }
}