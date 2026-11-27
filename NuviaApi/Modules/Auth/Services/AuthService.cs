namespace NuviaApi.Modules.Auth.Services;
using NuviaApi.Modules.Auth.DTOs;
using NuviaApi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using NuviaApi.Modules.Auth.Models;

public class AuthService
{
    private readonly NuviaDbContext _dbContexts;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AuthService(NuviaDbContext dbContext, IPasswordHasher<Usuario> passwordHasher)
    {
        _dbContexts = dbContext;
        _passwordHasher = passwordHasher;
    }

    public void Register(RegisterDto registerDto)
    {
        if (registerDto == null)
        {
            throw new ArgumentNullException(nameof(registerDto));
        }
        if( _dbContexts.Usuarios.Any(u => u.email == registerDto.email))
        {
            throw new InvalidOperationException("El correo electrónico ya está registrado.");

        }
        Usuario usuario = new Usuario
        {
            nombreUsuario = registerDto.nombreUsuario,
            email = registerDto.email,
            passwordHash = string.Empty // Se asignará el hash de la contraseña más adelante
        };
        usuario.passwordHash = _passwordHasher.HashPassword(usuario, registerDto.password);

        _dbContexts.Usuarios.Add(usuario);
        _dbContexts.SaveChanges();
    }

}
