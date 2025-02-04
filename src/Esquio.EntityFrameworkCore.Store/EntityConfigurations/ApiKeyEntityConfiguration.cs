﻿using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class ApiKeyEntityConfiguration : IEntityTypeConfiguration<Entities.ApiKeyEntity>
    {
        private readonly StoreOptions storeOption;

        public ApiKeyEntityConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<Entities.ApiKeyEntity> builder)
        {
            builder.ToTable(storeOption.ApiKeys);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(p => p.Name)
                .IsUnique();
            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(2000);
            builder.Property(p => p.Key)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(p => p.Key)
                .IsUnique();
        }
    }
}
