using Microsoft.EntityFrameworkCore;
using TrilhasApi.Models;

namespace TrilhasApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Trilha> Trilhas => Set<Trilha>();
    public DbSet<Desafio> Desafios => Set<Desafio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trilha>(entity =>
        {
            entity.ToTable("TRILHA");
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(t => t.Titulo)
                .HasColumnName("TITULO")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(t => t.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(t => t.Tema)
                .HasColumnName("TEMA")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(t => t.Nivel)
                .HasColumnName("NIVEL")
                .HasPrecision(1)
                .IsRequired();

            entity.Property(t => t.DataCriacao)
                .HasColumnName("DT_CRIACAO")
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            entity.Navigation(t => t.Desafios)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Desafio>(entity =>
        {
            entity.ToTable("DESAFIO");
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(d => d.TrilhaId)
                .HasColumnName("TRILHA_ID")
                .IsRequired();

            entity.Property(d => d.Pergunta)
                .HasColumnName("PERGUNTA")
                .HasMaxLength(500)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(d => d.AlternativaA)
                .HasColumnName("ALTERNATIVA_A")
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(d => d.AlternativaB)
                .HasColumnName("ALTERNATIVA_B")
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(d => d.AlternativaC)
                .HasColumnName("ALTERNATIVA_C")
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(d => d.AlternativaD)
                .HasColumnName("ALTERNATIVA_D")
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(d => d.RespostaCorreta)
                .HasColumnName("RESPOSTA_CORRETA")
                .HasMaxLength(1)
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            entity.Property(d => d.Pontos)
                .HasColumnName("PONTOS")
                .HasPrecision(3)
                .IsRequired();

            entity.HasOne(d => d.Trilha)
                .WithMany(t => t.Desafios)
                .HasForeignKey(d => d.TrilhaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DESAFIO_TRILHA");
        });
    }
}
