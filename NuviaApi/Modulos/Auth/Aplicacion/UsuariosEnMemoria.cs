using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;

namespace NuviaApi.Modulos.Auth.Aplicacion;

// Almacenamiento temporal: los usuarios se pierden al reiniciar la aplicación.
public sealed class UsuariosEnMemoria
{
    private readonly ConcurrentDictionary<string, Usuario> _usuarios = new(StringComparer.OrdinalIgnoreCase);
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public UsuarioAutenticadoDto? Registrar(RegisterUsuarioDto dto)
    {
        // El rol base lo asigna el servidor; el cliente no puede conceder privilegios.
        var datos = new UsuarioAutenticadoDto(Guid.NewGuid(), dto.nombre.Trim(), dto.correo.Trim(), 0);
        var usuario = new Usuario(datos, string.Empty);
        usuario = usuario with { PasswordHash = _passwordHasher.HashPassword(usuario, dto.contraseña) };

        return _usuarios.TryAdd(datos.Correo, usuario) ? datos : null;
    }

    public UsuarioAutenticadoDto? Autenticar(LoginUsuarioDto dto)
    {
        if (!_usuarios.TryGetValue(dto.correo.Trim(), out var usuario))
        {
            return null;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.contraseña);
        return resultado == PasswordVerificationResult.Failed ? null : usuario.Datos;
    }

    public UsuarioAutenticadoDto? BuscarPorCorreo(string correo)
    {
        return _usuarios.TryGetValue(correo, out var usuario) ? usuario.Datos : null;
    }

    private sealed record Usuario(UsuarioAutenticadoDto Datos, string PasswordHash);
}
