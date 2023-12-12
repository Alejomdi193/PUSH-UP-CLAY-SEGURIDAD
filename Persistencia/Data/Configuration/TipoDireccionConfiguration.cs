using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class TipoDireccionConfiguration : IEntityTypeConfiguration<TipoDireccion>
    {
        public void Configure(EntityTypeBuilder<TipoDireccion> builder)
        {
            builder.ToTable("TipoDireccion");

            builder.Property(p => p.Descripcion)
            .HasColumnName("Descripcion")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(400);
        }
    }
}