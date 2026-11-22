namespace NuviaApi.Modulos.Catalogo.Controllers;

using Microsoft.AspNetCore.Mvc;
using NuviaApi.Modulos.Catalogo.Aplicacion;

[ApiController]
[Route("api/alimentos")]
public sealed class AlimentosController : ControllerBase
{
    private readonly ListarAlimentos _listarAlimentos;

    public AlimentosController(ListarAlimentos listarAlimentos)
    {
        _listarAlimentos = listarAlimentos;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<AlimentoCatalogoDto>> Get()
    {
        return Ok(_listarAlimentos.Ejecutar());
    }
}
