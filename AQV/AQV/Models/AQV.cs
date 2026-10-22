using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AQV.Models;

public partial class AQV : DbContext
{
    public AQV()
    {
    }

    public AQV(DbContextOptions<AQV> options)
        : base(options)
    {
    }

    public virtual DbSet<Aluno> Alunos { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Ocorrencium> Ocorrencia { get; set; }

    public virtual DbSet<Ostatus> Ostatuses { get; set; }

    public virtual DbSet<Responsavel> Responsavels { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Turma> Turmas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CORA;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.HasKey(e => e.IdAluno).HasName("PK__Aluno__8092FCB30D2B8172");

            entity.ToTable("Aluno");

            entity.HasIndex(e => e.Numero, "UQ__Aluno__7E532BC65BB0C136").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Aluno__A9D105346605D046").IsUnique();

            entity.HasIndex(e => e.Cpf, "UQ__Aluno__C1F89731D171C072").IsUnique();

            entity.Property(e => e.IdAluno).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTurmaNavigation).WithMany(p => p.Alunos)
                .HasForeignKey(d => d.IdTurma)
                .HasConstraintName("FK_Aluno_Turma");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.IdFeedback).HasName("PK__Feedback__408FF10321CD9D29");

            entity.ToTable("Feedback");

            entity.Property(e => e.IdFeedback).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Anexo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DataEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Lido).HasDefaultValue(false);
            entity.Property(e => e.Mensagem)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOcorrenciaNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdOcorrencia)
                .HasConstraintName("FK__Feedback__IdOcor__02084FDA");
        });

        modelBuilder.Entity<Ocorrencium>(entity =>
        {
            entity.HasKey(e => e.IdOcorrencia).HasName("PK__Ocorrenc__831D9239C88761DC");

            entity.Property(e => e.IdOcorrencia).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataAbertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Descricao)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IdOstatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Prioridade)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(55)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Ocorrencia)
                .HasForeignKey(d => d.IdAluno)
                .HasConstraintName("FK__Ocorrenci__IdAlu__7A672E12");

            entity.HasOne(d => d.IdOstatusNavigation).WithMany(p => p.Ocorrencia)
                .HasForeignKey(d => d.IdOstatus)
                .HasConstraintName("FK__Ocorrenci__IdOst__7B5B524B");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ocorrencia)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Ocorrenci__IdUsu__7C4F7684");
        });

        modelBuilder.Entity<Ostatus>(entity =>
        {
            entity.HasKey(e => e.IdOstatus).HasName("PK__Ostatus__E76FF4A8824BD310");

            entity.ToTable("Ostatus");

            entity.Property(e => e.IdOstatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Responsavel>(entity =>
        {
            entity.HasKey(e => e.IdResponsavel).HasName("PK__Responsa__CDF1DCADCAC30A87");

            entity.ToTable("Responsavel");

            entity.HasIndex(e => e.Numero, "UQ__Responsa__7E532BC6A628A7DE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Responsa__A9D105344EBB2EDD").IsUnique();

            entity.Property(e => e.IdResponsavel).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Responsavels)
                .HasForeignKey(d => d.IdAluno)
                .HasConstraintName("FK__Responsav__IdAlu__6E01572D");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__TipoUsua__CA04062BBCAF6288");

            entity.ToTable("TipoUsuario");

            entity.Property(e => e.IdTipoUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Turma>(entity =>
        {
            entity.HasKey(e => e.IdTurma).HasName("PK__Turma__C1ECFFC943F81592");

            entity.ToTable("Turma");

            entity.Property(e => e.IdTurma).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97889CB7B5");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Numero, "UQ__Usuario__7E532BC6089B7676").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D105346B8F51E3").IsUnique();

            entity.Property(e => e.IdUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Numero)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .HasConstraintName("FK__Usuario__IdTipoU__628FA481");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
