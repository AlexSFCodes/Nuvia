namespace NuviaApi.Infrastructure.Persistence;
using NuviaApi.Modules.Auth.Models;
using Microsoft.EntityFrameworkCore;
public class NuviaDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    public NuviaDbContext(DbContextOptions<NuviaDbContext> options) : base(options)
    {
    }
}
