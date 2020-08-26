using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApiBlueBank.Models
{


    public class BANCOContext : IdentityDbContext<ApplicationUser>
    {
        public BANCOContext(DbContextOptions<BANCOContext> options)
          : base(options)
        { }

        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Cuentas> Cuentas { get; set; }
        public DbSet<Movimientos> Movimientos { get; set; }
        public DbSet<Sucursal> Sucursal { get; set; }
        public DbSet<TipoCuenta> TipoCuenta { get; set; }
        public DbSet<TipoDocumento> TipoDocumento { get; set; }
        public DbSet<TipoMovimiento> TipoMovimiento { get; set; }
    }
}
