using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class ContactoPersonaConfiguration : IEntityTypeConfiguration<ContactoPersona>
    {
        public void Configure(EntityTypeBuilder<ContactoPersona> builder)
        {
            builder.ToTable("ContactoPersona");


            builder.Property(p => p.Descripcion)
            .HasColumnName("Descripcion")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);

            builder.HasOne(p => p.Persona)
            .WithMany(p => p.ContactoPersonas)
            .HasForeignKey( p => p.IdPersonaFk);


            builder.HasOne(p => p.TipoContacto)
            .WithMany(p => p.ContactoPersonas)
            .HasForeignKey( p => p.IdTipoContactoFk);
        }
    }
}