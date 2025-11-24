using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ISIP523_Zasetsky;

public partial class БдДляПр7Context : DbContext
{
    public БдДляПр7Context()
    {
    }

    public БдДляПр7Context(DbContextOptions<БдДляПр7Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Detail> Details { get; set; }

    public virtual DbSet<DetailsGarage> DetailsGarages { get; set; }

    public virtual DbSet<Garage> Garages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ZASETSKY;Initial Catalog=БД_ДЛЯ_ПР7;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Detail>(entity =>
        {
            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.NameDetail).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<DetailsGarage>(entity =>
        {
            entity.ToTable("Details_Garage");

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DetailsID).HasColumnName("Details_ID");
            entity.Property(e => e.GarageID).HasColumnName("Garage_ID");

            entity.HasOne(d => d.Details).WithMany(p => p.DetailsGarages)
                .HasForeignKey(d => d.DetailsID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Details_Garage_Details");

            entity.HasOne(d => d.Garage).WithMany(p => p.DetailsGarages)
                .HasForeignKey(d => d.GarageID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Details_Garage_Garage");
        });

        modelBuilder.Entity<Garage>(entity =>
        {
            entity.ToTable("Garage");

            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.NameGarage).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
