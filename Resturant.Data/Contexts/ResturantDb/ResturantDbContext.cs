using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Resturant.Core;

namespace Resturant.Data;

public partial class ResturantDbContext : DbContext
{
    public ResturantDbContext()
    {
    }

    public ResturantDbContext(DbContextOptions<ResturantDbContext> options)
        : base(options)
    {
    }

   

    public virtual DbSet<LookUp> LookUps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

        modelBuilder.Entity<LookUp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LookUp_1");

            entity.ToTable("LookUp", "bs");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.NameEn).HasMaxLength(50);
        });

       

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
