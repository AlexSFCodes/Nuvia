namespace NuviaApi.Modules.Auth.DTOs;
using System.ComponentModel.DataAnnotations;
public class LoginDto
{
    [Required][EmailAddress]
    public required string email { get; set; }
    [Required][MinLength(6)]
    public required string password { get; set; }
}
