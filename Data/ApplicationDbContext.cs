using CancelamentoIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CancelamentoIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TiposCancelamentos> Tipos { get; set; }
        public DbSet<Voo> Voos { get; set; }
        public DbSet<Anexo> Anexos { get; set; }
        public DbSet<Cancelamento> Cancelamentos { get; set; }
    }
}
