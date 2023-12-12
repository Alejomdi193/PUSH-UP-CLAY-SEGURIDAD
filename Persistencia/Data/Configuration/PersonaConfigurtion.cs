using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class PersonaConfigurtion : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Persona");

            builder.Property(p => p.IdPersona)
            .HasColumnName("IdPersona")
            .HasColumnType("int")
            .IsRequired()
            .HasMaxLength(30);


            builder.Property(p => p.Nombre)
            .HasColumnName("Nombre")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(p => p.FechaRegistro)
            .HasColumnName("FechaRegistro")
            .HasColumnType("date")
            .IsRequired();

            builder.HasOne(p => p.TipoPersona)
            .WithMany(p => p.Personas)
            .HasForeignKey(p => p.IdTipoPersonaFk);

            builder.HasOne(p => p.CategoriaPersona)
            .WithMany(p => p.Personas)
            .HasForeignKey(p => p.IdCategortiaPersonaFk);

            builder.HasOne(p => p.Ciudad)
           .WithMany(p => p.Personas)
            .HasForeignKey(p => p.IdCiudadFk);


            
        }
    }
}



