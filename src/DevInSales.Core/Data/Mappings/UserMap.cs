﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DevInSales.Core.Entities;

namespace DevInSales.Core.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.Nome)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(u => u.DataNascimento)
                .HasColumnType("date")
                .IsRequired();
        }
    }

}
