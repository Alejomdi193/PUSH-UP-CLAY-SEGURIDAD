using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
    {
        public void Configure(EntityTypeBuilder<Departamento> builder)
        {
            builder.ToTable("Departamento");

            builder.Property(p => p.Nombre)
            .HasColumnName("Nombre")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);

            builder.HasOne(p => p.Pais)
            .WithMany(p => p.Departamentos)
            .HasForeignKey( p => p.IdPaisFk);
        }
    }
}