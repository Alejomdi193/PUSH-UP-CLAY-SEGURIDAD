using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class ContratoConfiguration : IEntityTypeConfiguration<Contrato>
    {
        public void Configure(EntityTypeBuilder<Contrato> builder)
        {
            builder.ToTable("Contrato");

            builder.Property(p => p.FechaContrato)
            .HasColumnName("FechaContrato")
            .HasColumnType("date")
            .IsRequired();

            builder.Property(p => p.FechaFin)
            .HasColumnName("FechaFin")
            .HasColumnType("date")
            .IsRequired();

            builder.HasOne(p => p.Cliente)
            .WithMany(p => p.ContratosCliente)
            .HasForeignKey(p => p.IdClienteFk);

            builder.HasOne(p => p.Empleado)
            .WithMany(p => p.ContratosEmpleado)
            .HasForeignKey(p => p.IdEmpleadoFk);

            builder.HasOne(p => p.Estado)
            .WithMany(p => p.Contratos)
            .HasForeignKey(p => p.IdEstadoFk);
        }
    }
}


