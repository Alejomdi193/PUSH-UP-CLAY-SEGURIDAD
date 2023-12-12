using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class DirPersonaConfiguration : IEntityTypeConfiguration<DirPersona>
    {
        public void Configure(EntityTypeBuilder<DirPersona> builder)
        {
            builder.ToTable("DireccionPersona");

            builder.Property(p => p.Direccion)
            .HasColumnName("Direccion")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(200);

            builder.HasOne(p => p.Persona)
            .WithMany(p => p.DirPersonas)
            .HasForeignKey( p => p.IdPersonaFk);

            builder.HasOne(p => p.TipoDireccion)
            .WithMany(p => p.DirPersonas)
            .HasForeignKey( p => p.IdTipoDireccionFk);

        }
    }
}