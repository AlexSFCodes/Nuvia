using System.ComponentModel.DataAnnotations;

namespace NuviaApi.Modulos.Auth.Aplicacion;

public sealed record LoginUsuarioDto(
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(254)]
    string correo,
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(256)]
    string contraseña);
