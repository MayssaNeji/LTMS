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

    public virtual DbSet<Agence> Agences { get; set; }

    public virtual DbSet<Chauffeur> Chauffeurs { get; set; }

    public virtual DbSet<Circuit> Circuits { get; set; }

    public virtual DbSet<Compte> Comptes { get; set; }

    public virtual DbSet<CompteHash> CompteHashes { get; set; }

    public virtual DbSet<Segment> Segments { get; set; }

    public virtual DbSet<Véhicule> Véhicules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MAYNEJI\\SQLEXPRESS;Database=LTMS;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agence>(entity =>
        {
            entity.HasKey(e => e.Nom);

            entity.ToTable("Agence");

            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Adresse)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.MatriculeFiscale).HasColumnName("matricule_fiscale");
            entity.Property(e => e.SiteInternet)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("site_internet");
        });

        modelBuilder.Entity<Chauffeur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chauffeu__3214EC27A2F25F26");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Agence)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.DateDeNaissance)
                .HasColumnType("date")
                .HasColumnName("Date_de_naissance");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Prenom)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.AgenceNavigation).WithMany(p => p.Chauffeurs)
                .HasForeignKey(d => d.Agence)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chauffeurs_Agence");
        });

        modelBuilder.Entity<Circuit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__circuit__3214EC27F6D5A8A3");

            entity.ToTable("circuit");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Agence)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.ContributionEmployé).HasColumnName("contribution_employé");
            entity.Property(e => e.CoutKm).HasColumnName("Cout_Km");
            entity.Property(e => e.NbKm).HasColumnName("Nb_Km");
            entity.Property(e => e.PointArrivée)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("point_arrivée");
            entity.Property(e => e.RefSapLeoni)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("REF_SAP_LEONI");

            entity.HasOne(d => d.AgenceNavigation).WithMany(p => p.Circuits)
                .HasForeignKey(d => d.Agence)
                .HasConstraintName("FK_circuit_Agence");
        });

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

        modelBuilder.Entity<Segment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Segment__3214EC274B8BC47C");

            entity.ToTable("Segment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CentreDeCout)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("centre_de_cout");
            entity.Property(e => e.ChefDeSegment)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("chef_de_segment");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.NomSegSapRef)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("Nom_seg_SAP_Ref");
            entity.Property(e => e.RhSegment)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("RH_Segment");
        });

        modelBuilder.Entity<Véhicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Véhicule__3214EC279BC95546");

            entity.ToTable("Véhicule");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Agence)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.DateDeMiseEnRoute)
                .HasColumnType("date")
                .HasColumnName("Date_de_mise_en_route");
            entity.Property(e => e.NomDeReference)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("Nom_de_reference");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.AgenceNavigation).WithMany(p => p.Véhicules)
                .HasForeignKey(d => d.Agence)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Véhicule_Agence");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
