using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuviaApi.Modulos.Auth.Aplicacion;
using NuviaApi.Modulos.CuentaHogar.Aplicacion;

namespace NuviaApi.Modulos.CuentaHogar.Controllers;

[ApiController]
[Authorize]
[Route("api/cuenta-hogar")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class CuentaHogarController : ControllerBase
{
    private readonly UsuariosEnMemoria _usuarios;
    private readonly IRepositorioHogares _hogares;

    public CuentaHogarController(UsuariosEnMemoria usuarios, IRepositorioHogares hogares)
    {
        _usuarios = usuarios;
        _hogares = hogares;
    }

    [HttpGet]
    [ProducesResponseType<CuentaHogarDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<CuentaHogarDto> Get()
    {
        var usuario = ObtenerUsuario();

        if (usuario is null)
        {
            return Unauthorized();
        }

        if (usuario.Rol != 0)
        {
            return Forbid();
        }

        return Ok(CrearRespuesta(usuario, _hogares.Obtener(usuario.Id)));
    }

    [HttpPut]
    [ProducesResponseType<CuentaHogarDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<CuentaHogarDto> Put([FromBody] ActualizarCuentaHogarDto dto)
    {
        var usuario = ObtenerUsuario();

        if (usuario is null)
        {
            return Unauthorized();
        }

        if (usuario.Rol != 0)
        {
            return Forbid();
        }

        var hogar = new HogarDto(
            dto.PresupuestoSemanal!.Value,
            dto.TamanoHogar!.Value,
            dto.Restricciones!
                .Select(alimento => alimento.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray());

        _hogares.Guardar(usuario.Id, hogar);

        return Ok(CrearRespuesta(usuario, hogar));
    }

    private UsuarioAutenticadoDto? ObtenerUsuario()
    {
        return Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var usuarioId)
            ? _usuarios.BuscarPorId(usuarioId)
            : null;
    }

    private static CuentaHogarDto CrearRespuesta(UsuarioAutenticadoDto usuario, HogarDto hogar)
    {
        return new CuentaHogarDto(
            usuario.Id,
            usuario.Nombre,
            usuario.Correo,
            hogar.PresupuestoSemanal,
            hogar.TamanoHogar,
            hogar.Restricciones);
    }
}
