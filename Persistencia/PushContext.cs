using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class PushContext : DbContext
    {
        public PushContext(DbContextOptions<PushContext> options) : base(options)
        {}

        public virtual  DbSet<Pais> Paises {get; set;}
        public virtual DbSet<Departamento> Departamentos {get; set;}
        public virtual DbSet<Ciudad> Ciudades {get; set;}
        public virtual DbSet<Contrato> Contratos {get; set;}
        public virtual DbSet<Estado> Estados {get; set;}
        public virtual DbSet<Persona> Personas {get; set;}
        public virtual DbSet<DirPersona> DirPersonas {get; set;}
        public virtual DbSet<Programacion> Programaciones {get; set;}
        public virtual DbSet<TipoPersona> TipoPersonas {get; set;}
        public virtual DbSet<TipoContacto> TipoContactos {get; set;}
        public virtual DbSet<ContactoPersona> ContactoPersonas {get; set;}
        public virtual DbSet<Turno> Turnos {get; set;}
        public virtual DbSet<TipoDireccion> TipoDirecciones {get; set;}
        public virtual DbSet<CategoriaPersona> CategoriaPersonas {get; set;}
        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<UserRol> UserRols { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}