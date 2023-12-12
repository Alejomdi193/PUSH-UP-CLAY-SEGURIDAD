using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class CategoriaPersonaConfiguration : IEntityTypeConfiguration<CategoriaPersona>
    {
        public void Configure(EntityTypeBuilder<CategoriaPersona> builder)
        {
            builder.ToTable("CategoriaPersona");

            builder.Property(p => p.Nombre)
            .HasColumnName("NombreCategoria")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);
        }
    }
}