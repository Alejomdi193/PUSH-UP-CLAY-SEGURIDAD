using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class TurnoConfiguration : IEntityTypeConfiguration<Turno>
    {
        public void Configure(EntityTypeBuilder<Turno> builder)
        {
            builder.ToTable("Turno");

            builder.Property(p => p.Nombre)
            .HasColumnName("Nombre")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);


            builder.Property(p => p.HoraTurnoI)
            .HasColumnName("HoraTurnoI")
            .HasColumnType("date")
            .IsRequired();


            builder.Property(p => p.HoraTurnoFinal)
            .HasColumnName("HoraTurnoFinal")
            .HasColumnType("date")
            .IsRequired();
        }
    }
}