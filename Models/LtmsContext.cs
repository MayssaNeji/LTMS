using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LTMS.Models;

public partial class LtmsContext : DbContext
{
    public LtmsContext()
    {
    }

    public LtmsContext(DbContextOptions<LtmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compte> Comptes { get; set; }

    public virtual DbSet<CompteHash> CompteHashes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MAYNEJI\\SQLEXPRESS;Database=LTMS;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Compte__3214EC2745C4BCE7");

            entity.ToTable("Compte");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompteHash>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompteHa__3214EC27AB67A77D");

            entity.ToTable("CompteHash");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Matricule);

            entity.ToTable("User");

            entity.Property(e => e.Matricule)
                .ValueGeneratedNever()
                .HasColumnName("matricule");
            entity.Property(e => e.Avatar)
                .HasColumnType("image")
                .HasColumnName("avatar");
            entity.Property(e => e.DateDeNaissance)
                .HasColumnType("date")
                .HasColumnName("date_de_naissance");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
