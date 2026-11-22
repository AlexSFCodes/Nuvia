namespace NuviaApi.Modulos.Auth.Aplicacion;

public sealed record UsuarioAutenticadoDto(
    Guid Id,
    string Nombre,
    string Correo,
    int Rol);
