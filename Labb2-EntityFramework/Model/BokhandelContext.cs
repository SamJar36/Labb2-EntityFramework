using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Labb2_EntityFramework.Model;

public partial class BokhandelContext : DbContext
{
    public BokhandelContext()
    {
    }

    public BokhandelContext(DbContextOptions<BokhandelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BonuspoängKonto> BonuspoängKontos { get; set; }

    public virtual DbSet<Butiker> Butikers { get; set; }

    public virtual DbSet<Böcker> Böckers { get; set; }

    public virtual DbSet<Författare> Författares { get; set; }

    public virtual DbSet<Kunder> Kunders { get; set; }

    public virtual DbSet<Lagersaldo> Lagersaldos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<BokhandelContext>().Build();

        var connectionString = new SqlConnectionStringBuilder()
        {
            ServerSPN = config["ServerName"],
            InitialCatalog = config["DatabaseName"],
            TrustServerCertificate = true,
            IntegratedSecurity = true
        }.ToString();
        optionsBuilder.UseSqlServer(connectionString);
    } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BonuspoängKonto>(entity =>
        {
            entity.HasKey(e => e.KontoId).HasName("PK__bonuspoä__E0CAF2000635F378");

            entity.ToTable("bonuspoängKonto");

            entity.Property(e => e.KontoId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("KontoID");
            entity.Property(e => e.KundId)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("KundID");
            entity.Property(e => e.Poäng).HasColumnName("poäng");

            entity.HasOne(d => d.Kund).WithMany(p => p.BonuspoängKontos)
                .HasForeignKey(d => d.KundId)
                .HasConstraintName("FK_bonuspoängKonto_kunder");
        });

        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__butiker__3214EC27CE21AF1E");

            entity.ToTable("butiker");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adress).HasMaxLength(100);
            entity.Property(e => e.Butiksnamn).HasMaxLength(100);
            entity.Property(e => e.Ort).HasMaxLength(100);

            entity.HasMany(d => d.Kontos).WithMany(p => p.Butiks)
                .UsingEntity<Dictionary<string, object>>(
                    "ButikBonusJunctionTable",
                    r => r.HasOne<BonuspoängKonto>().WithMany()
                        .HasForeignKey("KontoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__butik_bon__Konto__395884C4"),
                    l => l.HasOne<Butiker>().WithMany()
                        .HasForeignKey("ButikId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__butik_bon__Butik__3864608B"),
                    j =>
                    {
                        j.HasKey("ButikId", "KontoId").HasName("PK__butik_bo__ABDAC4DADF3C7EC6");
                        j.ToTable("butik_bonus_junctionTable");
                        j.IndexerProperty<int>("ButikId").HasColumnName("ButikID");
                        j.IndexerProperty<string>("KontoId")
                            .HasMaxLength(9)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("KontoID");
                    });
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__böcker__3BF79E03608B5D90");

            entity.ToTable("böcker");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.FörfattareId).HasColumnName("FörfattareID");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Språk).HasMaxLength(100);
            entity.Property(e => e.Titel).HasMaxLength(100);

            entity.HasOne(d => d.Författare).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FörfattareId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__böcker__Författa__151B244E");
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__författa__3214EC2733413BFA");

            entity.ToTable("författare");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Efternamn).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(100);
        });

        modelBuilder.Entity<Kunder>(entity =>
        {
            entity.HasKey(e => e.Personnummer).HasName("PK__kunder__245B03C0F0BA3108");

            entity.ToTable("kunder");

            entity.Property(e => e.Personnummer)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Efternamn).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(100);
        });

        modelBuilder.Entity<Lagersaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn }).HasName("PK__lagersal__1191B89431E53185");

            entity.ToTable("lagersaldo");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.Lagersaldos)
                .HasForeignKey(d => d.ButikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__lagersald__Butik__339FAB6E");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Lagersaldos)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__lagersaldo__ISBN__3493CFA7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
