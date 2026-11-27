using System.ComponentModel.DataAnnotations;

namespace NuviaApi.Modules.Auth.Models;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }
    [Required][MaxLength(50)]
    public required string nombreUsuario { get; set; }
    [Required][MaxLength(80)]
    public required string email { get; set; }
    [Required][MaxLength(100)]
    public required string passwordHash { get; set; }
}
