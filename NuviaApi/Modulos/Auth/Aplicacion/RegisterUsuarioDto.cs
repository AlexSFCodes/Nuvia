using System.ComponentModel.DataAnnotations;

namespace NuviaApi.Modulos.Auth.Aplicacion;

public sealed record RegisterUsuarioDto(
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    string nombre,
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(254)]
    string correo,
    [Range(0, 0, ErrorMessage = "El registro público solo permite el rol de usuario (0).")]
    int rol,
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(256, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 256 caracteres.")]
    string contraseña);
