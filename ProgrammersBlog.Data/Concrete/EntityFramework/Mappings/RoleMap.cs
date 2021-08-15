using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgrammersBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammersBlog.Data.Concrete.EntityFramework.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            // Primary key
            builder.HasKey(r => r.Id);

            // Index for "normalized" role name to allow efficient lookups
            builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

            // Maps to the AspNetRoles table
            builder.ToTable("AspNetRoles");

            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken(); //örneğin iki admin var ve aynı kullanıcı üzerinde ikiside değişiklik yapmaya çalışıyor.
                                                                            //bir admin kaydete bastığında diğeri kaydete basarsa ikinci admine exception fırlatır.

            // Limit the size of columns to use efficient database types
            builder.Property(u => u.Name).HasMaxLength(100);
            builder.Property(u => u.NormalizedName).HasMaxLength(100); // isim değerlerinin büyük harfe çevrildiği ve indexlendiği kısım.
            

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each Role can have many entries in the UserRole join table
            builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            // Each Role can have many associated RoleClaims
            builder.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.HasData(
               new Role
               {
                   Id = 1,
                   Name = "Admin",
                   NormalizedName = "ADMIN",
                   ConcurrencyStamp = Guid.NewGuid().ToString()
               },
               new Role
               {
                   Id = 2,
                   Name = "Editor",
                   NormalizedName = "EDITOR",
                   ConcurrencyStamp = Guid.NewGuid().ToString()
               });


            /////////////////////////////////////
            //ilk değer ataması fluent api için//
            #region
            //builder.HasData(new Role
            //{
            //    Id = 1,
            //    Name = "Admin",
            //    Description = "Admin Rolü, Tüm Haklara Sahiptir.",
            //    IsActive = true,
            //    IsDeleted = false,
            //    CretedByName = "InitialCreate",
            //    CreatedDate = DateTime.Now,
            //    ModifiedByName = "InitialCreate",
            //    ModifiedDate = DateTime.Now,
            //    Note = "Admin Rolüdür."
            //});
            #endregion
        }
    }
}
