using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Nombre)
                .HasColumnName("nombre")
                .HasComment("Nombre del usuario")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasComment("Correo del usuario")
                .HasColumnType("varchar")
                .HasMaxLength(255);

            builder.Property(p => p.Password)
                .HasMaxLength(255)
                .HasComment("Contraseña del usuario")
                .IsRequired();

            builder.HasMany(p => p.Rols)
                .WithMany(r => r.Users)
                .UsingEntity<UserRol>(

                    j => j
                        .HasOne(pt => pt.Rol)
                        .WithMany(t => t.UserRols)
                        .HasForeignKey(ut => ut.IdRolFK),


                    j => j
                        .HasOne(et => et.User)
                        .WithMany(et => et.UserRols)
                        .HasForeignKey(el => el.IdUserFK),

                    j =>
                    {
                        j.ToTable("user_rol");
                        j.HasKey(t => new { t.IdUserFK, t.IdRolFK });

                    });

            builder.HasMany(p => p.RefreshTokens)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.IdUserFK);
        }
    }
}