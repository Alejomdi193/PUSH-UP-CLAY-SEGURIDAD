using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class ProgramacionConfiguration : IEntityTypeConfiguration<Programacion>
    {
        public void Configure(EntityTypeBuilder<Programacion> builder)
        {
            builder.ToTable("Programacion");

            builder.HasOne(p => p.Contrato)
            .WithMany(p => p.Programaciones)
            .HasForeignKey( p => p.IdContratoFk);


            builder.HasOne(p => p.Turno)
            .WithMany(p => p.Programaciones)
            .HasForeignKey( p => p.IdTurnoFk);


            builder.HasOne(p => p.Empleado)
            .WithMany(p => p.Programaciones)
            .HasForeignKey( p => p.IdEmpleadoFk);
        }
    }
}