using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuviaApi.Modulos.Auth.Aplicacion;

namespace NuviaApi.Modulos.Auth.Controllers;

[ApiController]
[Route("api/auth")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class AuthController : ControllerBase
{
    private readonly UsuariosEnMemoria _usuarios;

    public AuthController(UsuariosEnMemoria usuarios)
    {
        _usuarios = usuarios;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType<UsuarioAutenticadoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public ActionResult<UsuarioAutenticadoDto> Register([FromBody] RegisterUsuarioDto dto)
    {
        var usuario = _usuarios.Registrar(dto);

        if (usuario is null)
        {
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "El correo ya está registrado.");
        }

        return StatusCode(StatusCodes.Status201Created, usuario);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginUsuarioDto dto)
    {
        var usuario = _usuarios.Autenticar(dto);

        if (usuario is null)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Correo o contraseña incorrectos.");
        }

        var identidad = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo)
            ],
            BearerTokenDefaults.AuthenticationScheme);

        return SignIn(new ClaimsPrincipal(identidad), BearerTokenDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType<UsuarioAutenticadoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UsuarioAutenticadoDto> Me()
    {
        var correo = User.FindFirstValue(ClaimTypes.Email);
        var usuario = correo is null ? null : _usuarios.BuscarPorCorreo(correo);

        if (usuario is null || User.FindFirstValue(ClaimTypes.NameIdentifier) != usuario.Id.ToString())
        {
            return Unauthorized();
        }

        return Ok(usuario);
    }
}
